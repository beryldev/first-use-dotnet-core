// using System.Collections.Generic;
// using System.Linq;
// using Moq;
// using Wrhs.Core;

// namespace Wrhs.WebApp.Tests
// {
//     public static class RepositoryFactory<T> where T : IEntity
//     {
//         public static IRepository<T> Make()
//         {
//              var items = new List<T>();
//             var mock = new Mock<IRepository<T>>();
//             mock.Setup(m=>m.Save(It.IsAny<T>()))
//                 .Callback((T item)=>{ items.Add(item); });

//             mock.Setup(m=>m.Get())
//                 .Returns(items);

//             mock.Setup(m=>m.GetById(It.IsAny<int>()))
//                 .Returns((int id)=>
//                 { 
//                     return items.ToArray().Where(item=>item.Id==id).FirstOrDefault(); 
//                 });

//             mock.Setup(m=>m.Update(It.IsAny<T>()))
//                 .Callback((T obj)=>
//                 {
//                     var p = items.Where(item=>item.Id==obj.Id).FirstOrDefault();
//                     if(p!=null)
//                     {
//                         items.Remove(p);
//                         items.Add(obj);
//                     }
//                 });

//             mock.Setup(m=>m.Delete(It.IsAny<T>()))
//                 .Callback((T obj) =>
//                 {
//                     items.Remove(obj);
//                 });

//             return mock.Object;
//         }
//     }
// }
