using Microsoft.Kinect;
using MyComponents.Enums;
using MyComponents.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MyLibrary.Util;
using Kinect_Center.Controller;

namespace Kinect_Center.UIFunctions
{
    /// <summary>
    /// Gerencia as funcionalidades referentes a interface com o usuário
    /// </summary>
    public static class UIFunctionsManager
    {
        #region [Constants]

        private const int OFF_SET = 30;

        #endregion end of [Constants]

        #region [Properties]

        /// <summary>
        /// Dictionary to select the color of each joint in the user skeleton mapped by Kinect
        /// </summary>
        public static Dictionary<JointType, SolidColorBrush> JointColors
        { get; private set; }

        #endregion end of [Properties]

        #region [Public Methods]

        /// <summary>
        /// This method create the dictionary of joint colors
        /// </summary>
        public static void GenerateColorsDictionary()
        {
            JointColors = new Dictionary<JointType, SolidColorBrush>();
            JointColors.Add(JointType.Head, Brushes.Crimson);
            JointColors.Add(JointType.ShoulderCenter, Brushes.IndianRed);
            JointColors.Add(JointType.Spine, Brushes.Salmon);
            JointColors.Add(JointType.HipCenter, Brushes.LightSalmon);

            JointColors.Add(JointType.ShoulderRight, Brushes.DarkBlue);
            JointColors.Add(JointType.ElbowRight, Brushes.MidnightBlue);
            JointColors.Add(JointType.WristRight, Brushes.MediumBlue);
            JointColors.Add(JointType.HandRight, Brushes.Blue);
            JointColors.Add(JointType.HipRight, Brushes.SkyBlue);
            JointColors.Add(JointType.KneeRight, Brushes.PowderBlue);
            JointColors.Add(JointType.AnkleRight, Brushes.PaleTurquoise);
            JointColors.Add(JointType.FootRight, Brushes.LightBlue);

            JointColors.Add(JointType.ShoulderLeft, Brushes.DarkOliveGreen);
            JointColors.Add(JointType.ElbowLeft, Brushes.DarkGreen);
            JointColors.Add(JointType.WristLeft, Brushes.Green);
            JointColors.Add(JointType.HandLeft, Brushes.ForestGreen);
            JointColors.Add(JointType.HipLeft, Brushes.YellowGreen);
            JointColors.Add(JointType.KneeLeft, Brushes.LimeGreen);
            JointColors.Add(JointType.AnkleLeft, Brushes.MediumSeaGreen);
            JointColors.Add(JointType.FootLeft, Brushes.LightGreen);
        }

        /// <summary>
        /// this method shows a dialog form with the question that was sent by parameter
        /// </summary>
        /// <param name="title">Title of dialog form</param>
        /// <param name="message">Message of dialog form</param>
        /// <returns>Returns true if user press "Yes" and false if user press "No"</returns>
        public static bool ShowQuestion(string title, string message)
        {
            DialogForm dialog = new DialogForm(DialogType.question, DialogButtons.yes_no, title, message);
            dialog.TimeToAutoClose = 5000;
            return dialog.ShowDialog().To<bool>();
        }

        /// <summary>
        /// this method shows a dialog form with the alert message that was sent by parameter
        /// </summary>
        /// <param name="title">Title of dialog form</param>
        /// <param name="message">Message of dialog form</param>
        public static void ShowAlert(string title, string message)
        {
            DialogForm dialog = new DialogForm(DialogType.alert, DialogButtons.ok, title, message);
            dialog.TimeToAutoClose = 5000;
            dialog.ShowDialog();
        }

        /// <summary>
        /// this method shows a dialog form with the error message that was sent by parameter
        /// </summary>
        /// <param name="title">Title of dialog form</param>
        /// <param name="message">Message of dialog form</param>
        public static void ShowError(string title, string message)
        {
            DialogForm dialog = new DialogForm(DialogType.error, DialogButtons.ok, title, message);
            dialog.TimeToAutoClose = 5000;
            dialog.Height += 300;
            dialog.Width += 300;
            dialog.ShowDialog();
        }

        /// <summary>
        /// this method shows a dialog form with the info message that was sent by parameter
        /// </summary>
        /// <param name="title">Title of dialog form</param>
        /// <param name="message">Message of dialog form</param>
        public static void ShowInfo(string title, string message)
        {
            DialogForm dialog = new DialogForm(DialogType.info, DialogButtons.ok, title, message);
            dialog.TimeToAutoClose = 5000;
            dialog.ShowDialog();
        }

        /// <summary>
        /// This method gets a joint of dictionary 
        /// </summary>
        /// <param name="color">Color of dictionary</param>
        /// <returns>Joint of dictionary</returns>
        public static JointType GetJointPerColor(Brush color)
        {
            return JointColors.First(joint => joint.Value == color).Key;
        }

        #endregion

        #region [Draw User Skeleton Methods]

        /// <summary>
        /// This method Draw the user skeleton in the canvas that was sent by parameter
        /// </summary>
        /// <param name="userSkeleton">User Skeleton</param>
        /// <param name="rootCanvas">Canvas to draw</param>
        /// <param name="showSkeleton">Indicates if must be drawn the user skeleton</param>
        /// <param name="highlightJoints">Indicates if must be highlighted the joints of user skeleton</param>
        /// <param name="color">Color for draw lines and joints, if this parameter is null the default color is Brushes.PaleVioletRed for lines and uses the <see cref="P:InputRegisterApp.UIFunctions.UIFunctionsManager.JointColors"/> dictionary</param>
        /// <param name="semiTransparent">Indicates if the skeleton must be drawn with 50% transparency</param>
        public static void DrawUserSkeleton(Skeleton userSkeleton, Canvas rootCanvas, bool showSkeleton, bool highlightJoints, Brush color = null, List<JointType> extraHighLightJointsList = null)
        {
            if (JointColors == null && highlightJoints)
                GenerateColorsDictionary();

            KinectSensor kinect = FrontController.Instance.KinectSensorManager.Kinect;

            if (showSkeleton)
            {
                CreateLineBetweenTwoJoints(kinect, JointType.Head, JointType.ShoulderCenter, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.ShoulderCenter, JointType.ShoulderRight, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.ShoulderCenter, JointType.ShoulderLeft, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.ShoulderCenter, JointType.Spine, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.ShoulderRight, JointType.ElbowRight, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.ElbowRight, JointType.WristRight, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.WristRight, JointType.HandRight, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.ShoulderRight, JointType.Spine, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.ShoulderLeft, JointType.ElbowLeft, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.ElbowLeft, JointType.WristLeft, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.WristLeft, JointType.HandLeft, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.ShoulderLeft, JointType.Spine, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.Spine, JointType.HipCenter, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.Spine, JointType.HipRight, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.Spine, JointType.HipLeft, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.HipCenter, JointType.HipLeft, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.HipCenter, JointType.HipRight, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.HipRight, JointType.KneeRight, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.KneeRight, JointType.AnkleRight, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.AnkleRight, JointType.FootRight, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.HipLeft, JointType.KneeLeft, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.KneeLeft, JointType.AnkleLeft, userSkeleton.Joints, rootCanvas, color);
                CreateLineBetweenTwoJoints(kinect, JointType.AnkleLeft, JointType.FootLeft, userSkeleton.Joints, rootCanvas, color);
            }

            if (highlightJoints)
                userSkeleton.Joints.ToList().ForEach(joint => DrawJoint(kinect, joint.JointType, userSkeleton.Joints, rootCanvas, color, extraHighLightJointsList));
        }


        /// <summary>
        /// This method create a line between two joints
        /// </summary>
        /// <param name="kinectScanner">Kinect Scanner that contains the user skeleton</param>
        /// <param name="sourceJoint">Joint that contains the starting coordinates for line</param>
        /// <param name="destinationJoint">Joint that contains the finishing coordinates for line</param>
        /// <param name="joints">Joints of User Skeleton</param>
        /// <param name="rootCanvas">Canvas to draw</param>
        /// <param name="color">Color for draw lines, if this parameter is null the default color is Brushes.PaleVioletRed</param>
        /// <param name="semiTransparent">Indicates if the skeleton must be drawn with 50% transparency</param>
        private static void CreateLineBetweenTwoJoints(KinectSensor kinect, JointType sourceJoint, JointType destinationJoint, JointCollection joints, Canvas rootCanvas, Brush color)
        {
            double sourceX;
            double sourceY;
            double destinationX;
            double destinationY;

            int offSet = OFF_SET;
            GetCoordinates(kinect, sourceJoint, joints, rootCanvas, out sourceX, out sourceY);
            GetCoordinates(kinect, destinationJoint, joints, rootCanvas, out destinationX, out destinationY);

            Line line = new Line
            {
                X1 = sourceX,
                Y1 = sourceY + offSet,
                X2 = destinationX,
                Y2 = destinationY + offSet,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                StrokeThickness = 4.0,
                Opacity = 100,
                Stroke = color != null ? color : Brushes.PaleVioletRed,
                StrokeLineJoin = PenLineJoin.Round
            };

            if (Math.Max(line.X1, line.X2) < rootCanvas.ActualWidth && Math.Min(line.X1, line.X2) > 0 && Math.Max(line.X1, line.X2) < rootCanvas.ActualWidth && Math.Max(line.Y1, line.Y2) < rootCanvas.ActualHeight && Math.Min(line.Y1, line.Y2) > 0)
                rootCanvas.Children.Add(line);
        }

        /// <summary>
        /// This method create a Ellipse of joint
        /// </summary>
        /// <param name="kinectScanner">Kinect Scanner that contains the user skeleton</param>
        /// <param name="joint">Joint to draw</param>
        /// <param name="joints">Joints of User Skeleton</param>
        /// <param name="rootCanvas">Canvas to draw</param>
        /// <param name="color">Color to draw a joint, if color is null this method uses the <see cref="P:InputRegisterApp.UIFunctions.UIFunctionsManager.JointColors"/> dictionary</param>
        /// <param name="semiTransparent">Indicates if the skeleton must be drawn with 50% transparency</param>
        private static void DrawJoint(KinectSensor kinect, JointType joint, JointCollection joints, Canvas rootCanvas, Brush color, List<JointType> extraHighLightJointsList = null)
        {
            double diameter = joint == JointType.Head ? 50 : 8;
            double centerX;
            double centerY;
            double leftOffSet;
            double topOffSet;
            int offSet = OFF_SET;
            int repeatTimes = 1;
            int extraHighLight = 15;
            GetCoordinates(kinect, joint, joints, rootCanvas, out centerX, out centerY);

            if (extraHighLightJointsList != null && extraHighLightJointsList.Contains(joint))
                repeatTimes = 2;

            for (int time = 0; time < repeatTimes; time++)
            {
                Ellipse ellipse = new Ellipse
                {
                    Width = diameter + (time * extraHighLight),
                    Height = diameter + (time * extraHighLight),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    StrokeThickness = 4.0,
                    Opacity = 100,
                    StrokeLineJoin = PenLineJoin.Round,
                    Tag = joint.To<int>()
                };

                if (time > 0)
                {
                    ellipse.Stroke = Brushes.LimeGreen;
                }
                else
                {
                    ellipse.Stroke = color != null ? color : JointColors[joint];
                }

                leftOffSet = centerX - ellipse.Width / 2;
                topOffSet = (centerY - ellipse.Height / 2) + offSet;

                if (topOffSet >= 0 && topOffSet < rootCanvas.ActualHeight && leftOffSet >= 0 && leftOffSet < rootCanvas.ActualWidth)
                {
                    Canvas.SetLeft(ellipse, leftOffSet);
                    Canvas.SetTop(ellipse, topOffSet);

                    rootCanvas.Children.Add(ellipse);
                }
            }
        }

        private static void GetCoordinates(KinectSensor kinect, JointType jointType, IEnumerable<Joint> joints, Canvas rootCanvas, out double x, out double y)
        {
            Joint joint = joints.First(j => j.JointType == jointType);
            Point coordinates2DPlan = GetJoint2DCoordinates(kinect, joint, rootCanvas.ActualWidth.To<int>(), rootCanvas.ActualHeight.To<int>());

            x = coordinates2DPlan.X;
            y = coordinates2DPlan.Y;
        }

        /// <summary>
        /// This method transfer the 3D coordinate system of skeleton joint to 2D plan coordinates
        /// </summary>
        /// <param name="joint">Joint that contains the coordinates</param>
        /// <param name="height">Height of panel to draw</param>
        /// <param name="width">Width of panel to draw</param>
        /// <returns>Point with X and Y position in a 2D plan</returns>
        private static System.Windows.Point GetJoint2DCoordinates(KinectSensor kinect, Joint joint, int width, int height)
        {
            DepthImagePoint depthPoint = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(joint.Position, kinect.DepthStream.Format);
            System.Windows.Point coordinates2DPlan = new System.Windows.Point { X = depthPoint.X, Y = depthPoint.Y };
            coordinates2DPlan.X = (coordinates2DPlan.X * width / 640);
            coordinates2DPlan.Y = (coordinates2DPlan.Y * height / 480);

            return coordinates2DPlan;
        }

        #endregion end of [Draw User Skeleton]
    }
}
