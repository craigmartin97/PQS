using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;

namespace PQS.Models.Models
{
    public class Categories : ICloneable, IDataErrorInfo
    {
        [Key]
        public int PK_Category_id { get; set; }
        [Required]
        public string CategoryName { get; set; }

        [Required]
        public int? Order { get; set; }

        #region Constructors
        public Categories() { } //required, dont delete
        public Categories(string categoryName, int? order)
        {
            CategoryName = categoryName;
            Order = order;
        }
        #endregion

        #region Clones
        public object Clone()
        {
            return new Categories()
            {
                PK_Category_id = PK_Category_id,
                CategoryName = CategoryName,
                Order = Order
            };
        }
        #endregion

        #region IDataErrorInfo
        public string Error
        {
            get
            {
                return "This field cannot be empty";
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName.Equals("CategoryName"))
                {
                    if (string.IsNullOrWhiteSpace(CategoryName))
                        return Error;
                }
                else if (columnName.Equals("Order"))
                {
                    if (Order == null)
                        return Error;
                }
                return null;
            }
        }

        #endregion
    }
}