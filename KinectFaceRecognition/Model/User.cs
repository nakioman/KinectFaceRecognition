namespace KinectFaceRecognition.Model
{
    public class User
    {
        public int Id { get; set; }

        public string NickName { get; set; }

        public RecognizedFace Face { get; set; }
    }
}