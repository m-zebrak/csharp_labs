# csharp_labs

# [1](1)
### [Task 1](1/t1/Program.cs)

Design an abstract class / record that defines the area, perimeter and returns the length of the sides of the triangle (any). The classes inheriting from this class are to be a triangle:
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

The employee class must contain at least 5 attributes. Every employee should have a workplace. The class should contain data validation (via the `Validate ()` method). The employee class should contain the methods `Show ()` and `IsMatch ()` 

#

### [Task 3](1/t3/Program.cs)

Add to the file from the second task reading and writing data in TXT, XML and JSON format (minimum one). Use the builder design pattern for this. The abstraction of a particular record can be passed to the directory constructor which has a `Save ()` method. 

#


### [Task 4](1/t4/Program.cs)
 
Extend the previous task with the ability to encrypt data with the Caesar method. The shift block should be larger than one. In order to store it, use the `ConfigurationManager` class (first add a reference to the` System.Configuration` library). 