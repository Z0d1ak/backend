using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gpn.Dto
{
    /// <summary>
    /// Контракт данных для результата поиска.
    /// </summary>
    /// <typeparam name="TItem">Тип возвращаемого элемента.</typeparam>
    public class SearchResponseDto<TItem>
    {
        public SearchResponseDto(int count, IEnumerable<TItem> items)
        {
            Count = count;
            Items = items;
        }

        /// <summary>
        /// Колчество элементов, удовлитвоярющих фильтрации.
        /// </summary>
        [Required]
        public int Count { get; set; }

        /// <summary>
        /// Элементы удовлитворяющие фильтрации.
        /// Количество элементов ограничено максимальным количеством элементов на странице.
        /// </summary>
        [Required]
        public IEnumerable<TItem> Items { get; set; }
    }
}
