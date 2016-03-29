namespace 
{
public interface  IOrdersRepository : IOrders<Orders>
Customers
Employees
Shippers
Orders Get (OrderID);
List<Orders>  GetAll();
}
}
namespace 
{
public interface  IOrdersRepository : IOrders<Orders>
ICustomersRepositoryCustomersRepository
IEmployeesRepositoryEmployeesRepository
IShippersRepositoryShippersRepository
Orders Get (OrderID);
List<Orders>  GetAll();
}
}
