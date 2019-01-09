/* 
 * Author : Marin Verstraete
 * Class  : TIS-E2B
 * Date   : 15.01.2018
 * Projet : FlappLeap
 */

using Microsoft.Xna.Framework;

namespace FlappLeap
{
    public class FlappLeapDrawableGameComponent : DrawableGameComponent
    {
        public FlappLeapGame FlappLeapGame { get => this.Game as FlappLeapGame; }

        public FlappLeapDrawableGameComponent(FlappLeapGame game) : base(game)
        {
        }
    }
}
