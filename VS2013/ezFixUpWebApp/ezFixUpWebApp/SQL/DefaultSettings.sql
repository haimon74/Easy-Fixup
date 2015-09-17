
-- INSERT DEFAULT ADMIN ACCOUNT (admin/admin) into Admins table 
 
INSERT INTO Admins 
(a_username,a_password) 
VALUES 
('admin','D033E22AE348AEB5660FC2140AEC35850C4DA997') 
GO 
 
-- INSERT default language
INSERT INTO Languages 
(l_language, l_active, l_predefined, l_order) 
VALUES 
('English',1, 1, 1) 
GO

INSERT INTO Languages 
(l_language, l_active, l_predefined, l_order, l_browser_languages, l_ipcountries) 
VALUES 
('Spanish',0 , 0, 2, 'es,es-ES,es-MX,es-AR,es-BO,es-CL,es-CO,es-CR,es-CU,es-DO,es-EC,es-SV,es-NI,es-PA,es-UY,es-VE', 'AR,BO,CL,CO,CR,CU,DO,EC,SV,GQ,GT,HN,MX,NI,PA,PY,PE,PR,ES,UY,VE') 
GO

INSERT INTO Languages 
(l_language, l_active, l_predefined, l_order, l_browser_languages, l_ipcountries) 
VALUES 
('German',0 , 0, 3, 'de,de-DE,de-AT,de-CH,de-LI,de-LU', 'DE,AT,CH,LI,LU') 
GO

INSERT INTO Languages 
(l_language, l_active, l_predefined, l_order, l_browser_languages, l_ipcountries) 
VALUES 
('French',0 , 0, 4, 'fr,fr-FR,fr-CA,fr-BE,fr-LU', 'FR,CA,BE,LU') 
GO

INSERT INTO Languages 
(l_language, l_active, l_predefined, l_order, l_browser_languages, l_ipcountries) 
VALUES 
('Italian',0 , 0, 5, 'it,it-IT,it-MT,it-MC,it-VA,it-SM', 'IT,MT,MC,VA,SM') 
GO

-- INSERT DEFAULT ADMIN ACCOUNT (admin/admin) into Users table 
 
INSERT INTO Users 
(u_username,u_password,u_email,u_name,u_gender,u_birthdate,u_active,u_profilevisible,u_receiveemails,u_paid_member) 
VALUES 
('admin','D033E22AE348AEB5660FC2140AEC35850C4DA997','admin@ezFixUp.com','Administrator',1,'1985-01-01',1,0,0,1) 
GO 

-- INSERT conact us and abous pages
INSERT INTO ContentPage
(cp_title, cp_content)
VALUES
('About us','')
GO

INSERT INTO ContentPage
(cp_title, cp_content)
VALUES
('Contact us','')
GO

INSERT INTO Settings (s_key,s_value) VALUES ('Misc_EnableFirstRunWizard', 'True')
GO