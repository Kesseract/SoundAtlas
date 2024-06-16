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

    public List<T> GetEntitiesByForeignKey<T>(int presetId, string Id) where T : class, new()
    {
        using (var context = new MyMusicAppContext())
        {
            return context.Set<T>().Where(p => EF.Property<int>(p, Id) == presetId).ToList();
        }
    }

    public List<T> GetAllEntities<T>() where T : class
    {
        using (var context = new MyMusicAppContext())
        {
            return context.Set<T>().ToList();
        }
    }

    public List<T> GetAllEntitiesIncluding<T>(params Expression<Func<T, object>>[] navigationPropertyPaths) where T : class
    {
        using (var context = new MyMusicAppContext())
        {
            IQueryable<T> query = context.Set<T>();
            foreach (var navigationPropertyPath in navigationPropertyPaths)
            {
                query = query.Include(navigationPropertyPath);
            }
            return query.ToList();
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

            // Null チェックを追加
            if (containsMethod == null)
            {
                throw new InvalidOperationException("The Contains method is not found.");
            }

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
