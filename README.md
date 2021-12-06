# csharp_labs

# [1](1)

### [Task 1](1/t1/Program.cs)

Design an abstract class / record that defines the area, perimeter and returns the length of the sides of the triangle (
any). The classes inheriting from this class are to be a triangle:

* equilateral (constructor with one parameter),
* isosceles (constructor with two parameters - two different sides),
* rectangular (constructor with two parameters - leg lengths).

> Overload the `ToString ()` method to contain the basic information set.

#

### [Task 2](1/t2/Program.cs)

Create a generic class employee file that allows you to:

* adding / removing,
* display,
* validation of existing employees,
* data search.

The employee class must contain at least 5 attributes. Every employee should have a workplace. The class should contain
data validation (via the `Validate ()` method). The employee class should contain the methods `Show ()` and `IsMatch ()`

#

### [Task 3](1/t3/Program.cs)

Add to the file from the second task reading and writing data in TXT, XML and JSON format (minimum one). Use the builder
design pattern for this. The abstraction of a particular record can be passed to the directory constructor which has
a `Save ()` method.

#

### [Task 4](1/t4/Program.cs)

Extend the previous task with the ability to encrypt data with the Caesar method. The shift block should be larger than
one. In order to store it, use the `ConfigurationManager` class (first add a reference to the` System.Configuration`
library).

***

# [2](2)

### [Task 1](2/t1/Program.cs)

Write a function (it is best to redefine `ToString`), which will return a string in the form of ONP for a given
expression in the C # programming language. For example:
for the expression `x = a - b * c`, function `x.ToString ()` will return `ab - c *` (no priority for the operation and
no parentheses).

The `OnpExpression` class should store its values to make it easy for the` ToString` method to return a result.

#

### [Task 2](2/t2/Program.cs)

Write an operator for `<` and `>` that checks whether the sum of the items in a list is greater or less than the second
list. For example, for `a = [1,2,3,4]` and `b = [20,30]` `a <b` should return `true`.

#

### [Task 3](2/t3/Program.cs)

There is a class `Student` that stores the following data: index number, age, gender, year of study, semester;
the `Degree` class stores data: subject, grade, year of credit, semester. Using * LINQ *.

* Combine data from the classes `Student` and` Degree`.
* View students whose age is above the average of the students per year.
* View students whose grade point average is greater than that of other students in the year.

***

# [3](3)

### [Task 1](3/t1/Program.cs)

Implement any three design patterns.

#

### [Task 2](3/t2/Program.cs)

Write an operator for `<` and `>` that checks whether the sum of the items in a list is greater or less than the second
list. For example, for `a = [1,2,3,4]` and `b = [20,30]` `a <b` should return `true`.

#

### [Task 3](3/t3/Program.cs)

Implement an observer class for the selected collection. When a property value changes, display what has changed on the
console.

***