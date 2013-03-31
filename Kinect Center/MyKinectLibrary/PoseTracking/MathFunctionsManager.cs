using Microsoft.Kinect;
using MyKinectLibrary.Enums;
using MyKinectLibrary.Model;
using MyKinectLibrary.Model.Auxiliary;
using MyLibrary.Util;
using System;

namespace MyKinectLibrary.PoseTracking
{
    /// <summary>
    /// This class manages all functions related to math functions
    /// </summary>
    internal static class MathFunctionsManager
    {
        #region [Public Methods]

        /// <summary>
        /// Calcula a lei dos cossenos em todos os planos
        /// </summary>
        /// <param name="subPose">SubPose que contém a posição dos joints</param>
        /// <returns>Retorna um objeto que contém o resultado do angulo em cada um dos planos 2D utilizando a lei dos cossenos</returns>
        internal static LawOfCosinesResult LawOfCosinesCalculation( SubPose subPose )
        {
            LawOfCosinesResult lawOfCosinesResult = new LawOfCosinesResult();
            lawOfCosinesResult.PlanXY = LawOfCosinesPlanCalculation( subPose.AuxiliaryJoint1.Position, subPose.CenterJoint.Position, subPose.AuxiliaryJoint2.Position, Axis.X, Axis.Y );
            lawOfCosinesResult.PlanXZ = LawOfCosinesPlanCalculation( subPose.AuxiliaryJoint1.Position, subPose.CenterJoint.Position, subPose.AuxiliaryJoint2.Position, Axis.X, Axis.Z );
            lawOfCosinesResult.PlanYZ = LawOfCosinesPlanCalculation( subPose.AuxiliaryJoint1.Position, subPose.CenterJoint.Position, subPose.AuxiliaryJoint2.Position, Axis.Y, Axis.Z );
            return lawOfCosinesResult;
        }

        /// <summary>
        /// Calcula o produto escalar dos jointts da subpose em um sistema de coordenadas 3D
        /// </summary>
        /// <param name="subPose">SubPose que contém a posição dos joints</param>
        /// <returns>Returna o valor do cálculo do produto escalar</returns>
        internal static double ScalarProductCalculation( SubPose subPose )
        {
            Vector3D vectorV = Vector3DBetweenTwoJoints(subPose.AuxiliaryJoint1,subPose.CenterJoint);
            Vector3D vectorW = Vector3DBetweenTwoJoints(subPose.AuxiliaryJoint2,subPose.CenterJoint);

            double resultVW = vectorV.X * vectorW.X + vectorV.Y * vectorW.Y + vectorV.Z * vectorW.Z;
            double resultModuleVW = Math.Sqrt( Math.Pow( vectorV.X, 2 ) + Math.Pow( vectorV.Y, 2 ) + Math.Pow( vectorV.Z, 2 ) ) * Math.Sqrt( Math.Pow( vectorW.X, 2 ) + Math.Pow( vectorW.Y, 2 ) + Math.Pow( vectorW.Z, 2 ) );
            double resultInRadians = Math.Acos( resultVW / resultModuleVW );

            double resultInDegrees = resultInRadians * 180 / Math.PI;

            return resultInDegrees;
        }

        /// <summary>
        /// Compara dois valores utilizando margem de erro
        /// </summary>
        /// <param name="value1">Primeiro valor</param>
        /// <param name="value2">Segundo valor</param>
        /// <param name="errorMargin">Margem de erro</param>
        /// <returns>Retorna true caso o valor seja aceito considerando a margem de erro e false caso o valor não seja aceito</returns>
        public static bool ComparisonWithErrorMargin( double value1, double value2, double errorMargin )
        {
            return value1 >= value2 - errorMargin && value1 <= value2 + errorMargin;
        }

        #endregion end of [Public Methods]


        #region [Private Methods]

        /// <summary>
        /// Calcula o angule entre os três joints enviados por parâmetros
        /// </summary>
        /// <param name="auxiliaryJointPosition">Posição do primeiro Joint</param>
        /// <param name="centerJointPosition">Posição do segundo Joint</param>
        /// <param name="auxiliary2JointPosition">Posição do terceiro Joint</param>
        /// <param name="firstAxis">Primeiro eixo para o cálculo</param>
        /// <param name="secondAxis">Segundo eixo para o cálculo</param>
        /// <returns>Retorna o resultado da lei dos cossenos em graus</returns>
        private static double LawOfCosinesPlanCalculation( SkeletonPoint auxiliaryJointPosition, SkeletonPoint centerJointPosition,SkeletonPoint auxiliary2JointPosition, Axis firstAxis, Axis secondAxis )
        {
            double firstAxisCenterJointPosition = centerJointPosition.GetType().GetProperty( firstAxis.ToString() ).GetValue( centerJointPosition, null ).To<double>();
            double secondAxisCenterJointPosition = centerJointPosition.GetType().GetProperty(secondAxis.ToString()).GetValue(centerJointPosition, null).To<double>();
            double firstAxisAuxiliaryJointPosition = auxiliaryJointPosition.GetType().GetProperty(firstAxis.ToString()).GetValue(auxiliaryJointPosition, null).To<double>();
            double secondAxisAuxiliaryJointPosition = auxiliaryJointPosition.GetType().GetProperty(secondAxis.ToString()).GetValue(auxiliaryJointPosition, null).To<double>();
            double firstAxisAuxiliaryJoint2Position = auxiliary2JointPosition.GetType().GetProperty(firstAxis.ToString()).GetValue(auxiliary2JointPosition, null).To<double>();
            double secondAxisAuxiliaryJoint2Position = auxiliary2JointPosition.GetType().GetProperty(secondAxis.ToString()).GetValue(auxiliary2JointPosition, null).To<double>();


            double a = StraightSegmentBetweenTwoJoints( firstAxisAuxiliaryJointPosition, secondAxisAuxiliaryJointPosition, firstAxisCenterJointPosition, secondAxisCenterJointPosition );
            double b = StraightSegmentBetweenTwoJoints( firstAxisCenterJointPosition, secondAxisCenterJointPosition, firstAxisAuxiliaryJoint2Position, secondAxisAuxiliaryJoint2Position );
            double c = StraightSegmentBetweenTwoJoints( firstAxisAuxiliaryJointPosition, secondAxisAuxiliaryJointPosition, firstAxisAuxiliaryJoint2Position, secondAxisAuxiliaryJoint2Position );

            double resultInRadians = Math.Acos( ( Math.Pow( a, 2 ) + Math.Pow( b, 2 ) - Math.Pow( c, 2 ) ) / ( 2 * a * b ) );
            double ResultInDegrees = resultInRadians * 180 / Math.PI;

            return ResultInDegrees;
        }

        /// <summary>
        /// Cálcula o vetor de distância entre dois joints
        /// </summary>
        /// <param name="joint1Axis1">Posição do primeiro eixo do primeiro joint</param>
        /// <param name="joint1Axis2">Posição do segundo eixo do primeiro joint</param>
        /// <param name="joint2Axis1">Posição do primeiro eixo do segundo joint</param>
        /// <param name="joint2Axis2">Posição do segundo eixo do segundo joint</param>
        /// <returns>Distância entre o valor de distância entre dois joints enviados por parâmetros</returns>
        private static double StraightSegmentBetweenTwoJoints( double joint1Axis1, double joint1Axis2, double joint2Axis1, double joint2Axis2 )
        {
            return Math.Sqrt( Math.Pow( joint1Axis1 - joint2Axis1, 2 ) + Math.Pow( joint1Axis2 - joint2Axis2, 2 ) );
        }

        /// <summary>
        /// Calcula o vetor 3D dos dois joints
        /// </summary>
        /// <param name="joint1">Primeiro joint</param>
        /// <param name="joint2">Segundo joint</param>
        /// <returns>Vector3D entre os joints enviados por parâmetros</returns>
        private static Vector3D Vector3DBetweenTwoJoints( Joint joint1, Joint joint2 )
        {
            Vector3D vector = new Vector3D();

            vector.X = Math.Sqrt( Math.Pow( joint1.Position.X - joint2.Position.X, 2 ) );
            vector.Y = Math.Sqrt( Math.Pow( joint1.Position.Y - joint2.Position.Y, 2 ) );
            vector.Z = Math.Sqrt( Math.Pow( joint1.Position.Z - joint2.Position.Z, 2 ) );
            
            return vector;
        }

        #endregion end of [Private Methods]

    }
}
