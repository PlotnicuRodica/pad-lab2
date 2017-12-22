﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
	public class Person
	{
		public string Name { get; set; }
		public int? Age { get; set; }
		public string Country { get; set; }

		public Person() { }

		public Person(string name, int? age, string country)
		{
			Name = name;
			Age = age;
			Country = country;
		}

		public override string ToString()
		{
			return $"\nName = {Name}\nAge = {Age}\nCountry = {Country}";
		}
	}
}
