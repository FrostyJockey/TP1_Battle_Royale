using Playmode.Ennemy.BodyParts;
using Playmode.Movement;

namespace Playmode.Ennemy.Strategies
{
    public class TurnAndShootStragegy : IEnnemyStrategy
    {
        private readonly Mover mover;
        private readonly HandController handController;

        public TurnAndShootStragegy(Mover mover, HandController handController)
        {
            this.mover = mover;
            this.handController = handController;
        }

        public void Act()
        {
            //mover.Rotate(Mover.Clockwise);

<<<<<<< HEAD
            //handController.Use();
=======
           // handController.Use();
>>>>>>> f14758ce5bad900ee8eb821e07c221b091949dd1
        }
    }
}