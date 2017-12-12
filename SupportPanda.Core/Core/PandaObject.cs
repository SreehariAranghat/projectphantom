using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class PandaObject
    {
        public int Id { get; set; }

        [JsonIgnore]
        [BsonIgnore]
        public User CreatedUser { get; set; }

        [JsonIgnore]
        [BsonIgnore]
        public User ModifiedUser { get; set; }

        [JsonIgnore]
        [BsonIgnore]
        public User DeletedUser { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual bool IsValid()
        {
            return !ValidationHelper.ValidateEntity<PandaObject>(this).HasError;
        }

        public virtual IList<ValidationResult> Validate()
        {
            return ValidationHelper.ValidateEntity<PandaObject>(this).Errors;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
