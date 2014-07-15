-- VERSION 0.1

USE $MAIN$;

/*****************************************************************************/

-- Adding keys, token and pwd
INSERT INTO config (`name`, `type`, `caption`, `hint`, `value`, `optional`) VALUES ('Github-client-id', 'string', 'Github client Id', 'Enter the value of the Github client Id', 'e9d253230620c913d897', '0');
INSERT INTO config (`name`, `type`, `caption`, `hint`, `value`, `optional`) VALUES ('Github-client-secret', 'string', 'Github client secret', 'Enter the value of the Github client secret', '763201404f5fca0ad5f9fef558eec94bee2a28c6', '0');
INSERT INTO config (`name`, `type`, `caption`, `hint`, `value`, `optional`) VALUES ('Github-SSHkey-sandbox', 'string', 'Github ssh key', 'Enter the value of the Github ssh key', 'ssh-rsa AAAAB3NzaC1yc2EAAAABIwAAAQEAvVT6BtbzDUKiipcmpEyyvanXyLJ1WGUy5VWVlG7LR8owQ8GTE3Ct5fljb', '0');
-- RESULT
