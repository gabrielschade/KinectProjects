using Microsoft.Kinect;
using MyKinectLibrary.Delegates;
using MyKinectLibrary.Enums;
using MyKinectLibrary.Model;
using MyKinectLibrary.Model.Auxiliary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKinectLibrary.PoseTracking
{
    public class PoseRecognitionEngine
    {
        #region [Atributos]

        private readonly PoseRecognitionType _recognitionType;

        #endregion [Atributos]

        #region [Propriedades]

        public List<Pose> Poses { get; set; }

        #endregion [Propriedades]

        #region [Eventos]

        public event PoseStatusChanged PoseStatusChanged;

        #endregion [Eventos]

        #region [Construtores]

        public PoseRecognitionEngine(PoseRecognitionType recognitionType)
        {
            _recognitionType = recognitionType;
            this.Poses = new List<Pose>();
        }

        #endregion [Construtores]

        #region [Métodos Públicos]

        /// <summary>
        /// Este método analisa o esqueleto do usuário e compara com a lista de poses em memória da engine
        /// <para>
        /// Em primeiro lugar é checado se existe alguma pose com o estado "in_progress", caso exista este método verifica se a pose permanece válida.
        /// Caso a pose permaneça válida, este método analisa se o tempo para reconhecimento da pose foi completado, caso completado, o estado da pose é alterado
        /// </para>
        /// <para>
        /// Após isso é verificado se existe uma pose em algum estado aceito, caso exista, é verificado se a pose permaneça válida.
        /// Caso a pose não esteja mais válida, o método invoca o evento de pose interrompida
        /// </para>
        /// <para>
        /// Finalmente é verificado se alguma das poses com o estado "not_start" é reconhecida, caso sim, o seu estado é marcado para "in_progress"
        /// </para>
        /// </summary>
        /// <param name="userSkeleton">Esqueleto do usuário</param>
        public void PoseTracking(Skeleton userSkeleton)
        {
            if (userSkeleton == null)
            {
                CallPoseEvent(null, PoseEventStatus.interrupted);
                return;
            }

            Pose hypotheticalPose = null;
            List<Pose> inProgressPose = Poses.Where(pose => pose.Status == PoseStatus.in_progress || pose.Status == PoseStatus.accepted).ToList<Pose>();
            bool checkAllPoses = inProgressPose.Count == 0;
            if (inProgressPose.Count > 0)
            {
                hypotheticalPose = inProgressPose.First();
                bool poseIsRecognized = PoseTest(userSkeleton, hypotheticalPose, _recognitionType);

                if (hypotheticalPose.Status == PoseStatus.in_progress)
                {
                    if (poseIsRecognized)
                    {
                        TestPoseInProgress(hypotheticalPose);
                    }
                    else
                    {
                        RestartHypotheticalPoseTracking(hypotheticalPose);
                        checkAllPoses = true;
                    }
                }
                else if (hypotheticalPose.Status == PoseStatus.accepted)
                {
                    if (!poseIsRecognized)
                    {
                        RestartHypotheticalPoseTracking(hypotheticalPose);
                        checkAllPoses = true;
                        CallPoseEvent(hypotheticalPose, PoseEventStatus.interrupted);
                    }
                }
            }

            if (checkAllPoses)
            {
                string hypotheticalPoseName = hypotheticalPose == null ? "" : hypotheticalPose.Name;
                List<Pose> startingPoses = Poses.Where(pose => pose.Name != hypotheticalPoseName && PoseTest(userSkeleton, pose, _recognitionType)).ToList<Pose>();
                if (startingPoses.Count > 0)
                {
                    Pose poseToStart = startingPoses.First();
                    poseToStart.Status = PoseStatus.in_progress;
                    CallPoseEvent(poseToStart, PoseEventStatus.in_progress);
                    TestPoseInProgress(poseToStart);
                }

            }


        }

        #endregion [Métodos Públicos]

        #region [Métodos Privados]

        /// <summary>
        /// Reinicia o current frame da pose hipotética e marca o estado como "not_started"
        /// </summary>
        /// <param name="hypotheticalPose">Hypothetical pose</param>
        private void RestartHypotheticalPoseTracking(Pose hypotheticalPose)
        {
            hypotheticalPose.CurrentFrame = 0;
            hypotheticalPose.Status = PoseStatus.not_started;
            CallPoseEvent(hypotheticalPose, PoseEventStatus.interrupted);
        }

        /// <summary>
        /// Incrementa o "Current frame" da pose hipotética e dispara o evento de pose reconhecida caso o  Waiting time seja alcançado.
        /// </summary>
        /// <param name="hypotheticalPose">Pose hipotética</param>
        private void TestPoseInProgress(Pose hypotheticalPose)
        {
            hypotheticalPose.CurrentFrame += 1;
            if (hypotheticalPose.WaitingTime == hypotheticalPose.CurrentFrame)
            {
                hypotheticalPose.Status = PoseStatus.accepted;
                CallPoseEvent(hypotheticalPose, PoseEventStatus.recognized);
            }
        }

        /// <summary>
        /// Dispara o evento de reconhecimento de pose
        /// </summary>
        /// <param name="pose">Pose que teve o estado alterado</param>
        /// <param name="status">Estado da pose</param>
        private void CallPoseEvent(Pose pose, PoseEventStatus status)
        {
            if (PoseStatusChanged != null)
                PoseStatusChanged(pose, status);
        }

        /// <summary>
        /// Método para comprar o esqueleto corrente com a pose enviada por parâmetro
        /// </summary>
        /// <param name="userSkeleton">Esqueleto do usuário em tempo de execução</param>
        /// <param name="poseForTest">Pose para comparar</param>
        /// <param name="recognitionType">Indica o algortimo que será utilizado para comparação</param>
        /// <returns>Retorna true caso a pose seja reconhecida, caso contrário retorna false</returns>
        private bool PoseTest(Skeleton userSkeleton, Pose poseForTest, PoseRecognitionType recognitionType)
        {
            bool returnValue = true;

            foreach (SubPose subPose in poseForTest.SubPoses)
            {
                returnValue &= SubPoseTest(subPose, userSkeleton, recognitionType);
                if (!returnValue)
                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// Método para comprar o esqueleto corrente com a subpose enviada por parâmetro
        /// </summary>
        /// <param name="userSkeleton">Esqueleto do usuário em tempo de execução</param>
        /// <param name="subPoseForTest">SubPose para comparar</param>
        /// <param name="recognitionType">Indica o algortimo que será utilizado para comparação</param>
        /// <returns>Retorna true caso a subpose seja reconhecida, caso contrário retorna false</returns>
        private bool SubPoseTest(SubPose subPoseForTest, Skeleton userSkeleton, PoseRecognitionType recognitionType)
        {
            Joint centerJoint = userSkeleton.Joints[subPoseForTest.CenterJoint.JointType];
            Joint auxiliaryJoint1 = userSkeleton.Joints[subPoseForTest.AuxiliaryJoint1.JointType];
            Joint auxiliaryJoint2 = userSkeleton.Joints[subPoseForTest.AuxiliaryJoint2.JointType];
            SubPose userSubPose = new SubPose(centerJoint, auxiliaryJoint1, auxiliaryJoint2);

            bool returnValue = true;

            if (recognitionType == PoseRecognitionType.scalarProduct)
            {
                double scalarProductResult = MathFunctionsManager.ScalarProductCalculation(userSubPose);

                returnValue = MathFunctionsManager.ComparisonWithErrorMargin(scalarProductResult, subPoseForTest.Angle.ScalarProductResult, subPoseForTest.AngleMarginError);
            }
            else
            {
                LawOfCosinesResult lawOfCosinesResult = MathFunctionsManager.LawOfCosinesCalculation(userSubPose);
                returnValue &= MathFunctionsManager.ComparisonWithErrorMargin(subPoseForTest.Angle.LawOfCosinesResult.PlanXY, lawOfCosinesResult.PlanXY, subPoseForTest.AngleMarginError);
                returnValue &= MathFunctionsManager.ComparisonWithErrorMargin(subPoseForTest.Angle.LawOfCosinesResult.PlanXZ, lawOfCosinesResult.PlanXZ, subPoseForTest.AngleMarginError);
                returnValue &= MathFunctionsManager.ComparisonWithErrorMargin(subPoseForTest.Angle.LawOfCosinesResult.PlanYZ, lawOfCosinesResult.PlanYZ, subPoseForTest.AngleMarginError);
            }

            return returnValue;
        }


        #endregion end of [Métodos Privados]

    }
}
