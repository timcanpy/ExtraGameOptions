namespace Common_Classes
{
    public class EnumLibrary
    {
        #region Injuries
        //Progression 100,85,75,50,25,0
        public enum InjuryTypes
        {
            Healthy,
            Bruised,
            Sprained,
            Hurt,
            Injured,
            Broken
        };
        #endregion

        #region Conditional Replacements
        /*Four types of conditionals (Enum)
         1) String
            Checks the string value of objects used in the match.
         2) Percent
             Checks the percentage value of objects used in the match. 
         3) Flag
            Checks whether specific conditions about the match are true.
         4) None
             This always forces the replacement check to occur.
         */
        public enum Conditional
        {
            String,
            Numeric,
            Flag,
            None
        };

        /*Three Operands (Enum)
        1) Equals
        2) Greater Than
        3) Less Than
        */
        public enum Operands
        {
            Equals,
            GreaterThan,
            LessThan
        };
        #endregion

        #region Reversals
        public enum Executor
        {
            Defender,
            Attacker
        };

        public enum MoveType
        {
            FGrapple,
            Corner
        };
        #endregion

        #region Ukemi Notification

        public enum NotificationType
        {
            Wrestler,
            Promotion
        };

        #endregion
    }
}
