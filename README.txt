Say you have stored procedure that asks for the name and age and returns every customer that matches. Lets create a poco class that represents the criteria to be passed in to the procedure. We will override the tostring method to return the name of the stored procedure.
	
	public class CustomersByNameAndAgeQuery {
		public string Name {get;set;}
		public int Age {get;set;}
		
		public override string ToString() {
			return "dbo.uspGetCustomersByNameAndAge"
		}
	}
	
Lets assume the procedure returns all customers with id, name, age & Dob. Suppose we have a CLR type matches exactly what is returned.

	public class Customer {
		public int Id {get;set;}
		public string Name {get;set;}
		public int Age {get;set;}
		public DateTime Dob {get;set;}
	}
	
Since we have a class that by convention maps to the stored procedure and we have a return type that is the same too, we can simply do the following to call the procedure.

	IAdoSessoin<Customer> session {get;set;} // reference obtained via property or ctor injection
	
	var customers = session.Execute(new CustomersByNameAndAgeQuery {Name = 'Mary', Age = 34});
	
We did not have to write any code to add parameters to the procedure, map returned data reader columns to the CLR type, or even have to manage with opening and closing the connection.

Lets say for arguments sake the Dob stored as a long in the database. We now have a property mismatch since the CLR type declares it as an DateTime.

By implementing the following class we can handle this mapping exception.

	public class DateOfBirthImpedanceMapper : PropertyImpedanceMapper<Customer>
	{
		public override Expression<Func<Customer, object>> PropertyExpression 
		{ 
			get { return x => x.Dob; }
		}
		
		protected override void HandlePropertyImpedance(ref object value)
		{
			value = new DateTime((long)value);
		}
	}

Note: This is not the best way to handle mapping. The next version will have support so that an impedance handler class will take in a property info and the data reader, so it can assign the property a value by building it from one or many values from the passed in reader.

Also note for Adotic to work the developer is required to provide a binding for a Func<DbConnection>. The next vesrion will have some of support such that with no setup from the developer the connection can be created following some convetion that can be overriden easily.

Also Adotic will open and close the connection for every call to Execute. The next version will have support for multiple calls to use the same connection. 
