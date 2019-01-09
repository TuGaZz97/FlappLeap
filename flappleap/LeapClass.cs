using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;
using Microsoft.Xna.Framework;

namespace FlappLeap
{
    public class LeapClass
    {
        private bool clapped = false;
        private int counter = 0;
        int clap_count = 1;

        public LeapClass()
        {

        }

        public Point mousePosition(Frame frame)
        {
            // App dimension
            int appWidth = 1920;
            int appHeight = 1080;

            // Box interaction
            InteractionBox iBox = frame.InteractionBox;

            // Getting the hand and then the finger
            Hand hand = frame.Hands[0];
            Finger finger = hand.Fingers[1];

            Vector leapPoint = finger.StabilizedTipPosition;
            Vector normalizedPoint = iBox.NormalizePoint(leapPoint, false);

            float appX = normalizedPoint.x * appWidth;
            float appY = (1 - normalizedPoint.y) * appHeight;

            Point pt = new Point();
            pt.X = (int)appX;
            pt.Y = (int)appY;

            return pt;
        }

        public bool detectClap(Frame frame)
        {
            // Get a list of the gestures
            GestureList gsl = frame.Gestures();

            // Get the palms position
            Vector RightHandPos = frame.Hands[0].PalmPosition;
            Vector LeftHandPos = frame.Hands[1].PalmPosition;

            // Get the distance between the two palms
            float DistanceBetweenHands = RightHandPos.DistanceTo(LeftHandPos);

            // Get the inclination of the palms
            float RightHand = frame.Hands[0].PalmNormal.Roll;
            float LeftHand = frame.Hands[1].PalmNormal.Roll;

            // CLAP DETECTION

            // Claps must have 20 frames between them
            if (clapped == true)
            {
                counter++;

                if (counter == 20)
                {
                    clapped = false;
                    counter = 0;
                }
            }
            else if (((RightHand < -0.8f && RightHand > -2.5f) == true) & ((LeftHand > 0.8f && LeftHand < 2.5f) == true) & (DistanceBetweenHands < 60) == true)
            {
                clap_count++;
                clapped = true;
                return true;
            }

            // END OF CLAP DETECTION

            for (int i = 0; i < gsl.Count(); i++)
            {
                Gesture g = gsl[i];

                switch (g.Type)
                {
                    case Gesture.GestureType.TYPECIRCLE:
                        // tbxEvents.Text += "Cercle" + Environment.NewLine;
                        break;
                    case Gesture.GestureType.TYPESWIPE:
                        // tbxEvents.Text += "Swipe" + Environment.NewLine;
                        break;
                    case Gesture.GestureType.TYPE_KEY_TAP:
                        // tbxEvents.Text += "Key Tap" + Environment.NewLine;
                        break;
                    case Gesture.GestureType.TYPE_SCREEN_TAP:
                        return true;
                        //break;
                }
            }

            return false;
        }
    }
}
