using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Ez.Biz
{
    public class UpdateSql<TModel>
    {
        public void UpdateField<TProperty>(Expression<Func<TModel,TProperty>> expression)
        { 
        
        }
        public void Condation<TProperty>(Expression<Func<TModel, TProperty>> expression)
        { 
        
        }
        StringBuilder sql = new StringBuilder("update ");


    }
}
