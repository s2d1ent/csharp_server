namespace AMES{
    public class BackUp{
        private string _confFile;
        /*
            full-backup, differential-backuup
            syntax: full;differential
        */
        public string Types;
        /* 
            if Types is "full;differetial" then
            syntax Files: /dev/sda,/dev/sdb;/usr/www,/etc/ then
            /dev/sda,/dev/sdb is full-backup and /usr/www,/etc/ is differential
        */
        public string Files;
        /*
            synta: 00:00;00:00
        */
        public string Times;
    }
}