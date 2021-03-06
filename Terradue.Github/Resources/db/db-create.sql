-- VERSION 0.1

USE $MAIN$;

/*****************************************************************************/

-- Creating usr_github table
CREATE TABLE usr_github (
    id int unsigned NOT NULL,
    username varchar(50) COMMENT 'Username on github',
	token varchar(50) COMMENT 'Token to access github',
	email varchar(50) COMMENT 'User email on github',
	CONSTRAINT pk_usrgithub PRIMARY KEY (id),
	CONSTRAINT u_usrgithub UNIQUE (id),
    CONSTRAINT fk_usrgithub_usr FOREIGN KEY (id) REFERENCES usr(id) ON DELETE CASCADE
) Engine=InnoDB COMMENT 'User github';
-- RESULT

/*****************************************************************************/

-- Adding keys, token and pwd
INSERT INTO config (`name`, `type`, `caption`, `hint`, `optional`) VALUES ('Github-client-name', 'string', 'Github client name', 'Enter the value of the Github client name', '0');
INSERT INTO config (`name`, `type`, `caption`, `hint`, `optional`) VALUES ('Github-client-id', 'string', 'Github client Id', 'Enter the value of the Github client Id', '0');
INSERT INTO config (`name`, `type`, `caption`, `hint`, `optional`) VALUES ('Github-client-secret', 'string', 'Github client secret', 'Enter the value of the Github client secret', '0');
-- RESULT

/*****************************************************************************/

-- Adding rows in usr_github
INSERT INTO usr_github (`id`) SELECT id FROM usr;
-- RESULT

/*****************************************************************************/
