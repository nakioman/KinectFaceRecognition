using System.Data.Entity;

namespace KinectFaceRecognition.Model
{
    public class FaceRecognitionContext : DbContext
    {
        public IDbSet<User> Users { get; set; } 
    }
}