using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
public class DatabaseService
{
    public void AddEntity<T>(T entity) where T : class
    {
        using (var context = new MyMusicAppContext())
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }
    }

    public T? GetEntityById<T>(int id) where T : class
    {
        using (var context = new MyMusicAppContext())
        {
            return context.Set<T>().Find(id);
        }
    }

    public List<T> GetAllEntities<T>() where T : class
    {
        using (var context = new MyMusicAppContext())
        {
            return context.Set<T>().ToList();
        }
    }

    public List<T> GetAllEntitiesIncluding<T, TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath) where T : class
    {
        using (var context = new MyMusicAppContext())
        {
            return context.Set<T>().Include(navigationPropertyPath).ToList();
        }
    }

    public List<T> SearchEntities<T>(string columnName, string searchText) where T : class
    {
        using (var context = new MyMusicAppContext())
        {
            var query = context.Set<T>().AsQueryable();

            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, columnName);
            var searchTextExpression = Expression.Constant(searchText);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var containsExpression = Expression.Call(property, containsMethod, searchTextExpression);

            var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
            query = query.Where(lambda);

            return query.ToList();
        }
    }

    public void UpdateEntity<T>(T entity) where T : class
    {
        using (var context = new MyMusicAppContext())
        {
            context.Set<T>().Update(entity);
            context.SaveChanges();
        }
    }

    public void DeleteEntities<T>(IEnumerable<T> entities) where T : class, new()
    {
        using (var context = new MyMusicAppContext())
        {
            context.Set<T>().RemoveRange(entities);
            context.SaveChanges();
        }
    }
}
