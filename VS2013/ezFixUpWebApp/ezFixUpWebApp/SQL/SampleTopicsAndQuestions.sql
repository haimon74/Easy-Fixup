PRINT 'INSERTING DATA INTO TABLE ProfileTopics'
ALTER TABLE [ProfileTopics] NOCHECK CONSTRAINT ALL
SET IDENTITY_INSERT [ProfileTopics] ON
INSERT INTO [ProfileTopics] ([pt_id],[pt_name],[pt_priority],[pt_editcolumns],[pt_viewcolumns]) VALUES (1,'Basic Information',1,2,2)
INSERT INTO [ProfileTopics] ([pt_id],[pt_name],[pt_priority],[pt_editcolumns],[pt_viewcolumns]) VALUES (2,'Extra Information',2,1,1)
SET IDENTITY_INSERT [ProfileTopics] OFF
ALTER TABLE [ProfileTopics] CHECK CONSTRAINT ALL

PRINT 'INSERTING DATA INTO TABLE ProfileQuestions'
ALTER TABLE [ProfileQuestions] NOCHECK CONSTRAINT ALL
SET IDENTITY_INSERT [ProfileQuestions] ON
INSERT INTO [ProfileQuestions] ([pq_id],[pt_id],[pq_name],[pq_altname],[pq_description],[pq_hint],[pq_edit_style],[pq_show_style],[pq_search_style],[pq_required],[pq_priority],[pq_requires_approval],[pq_visible_male],[pq_visible_female],[pq_visible_couple]) VALUES (1,1,'Height','Height','','',5,2,0,1,1,0,1,1,1)
INSERT INTO [ProfileQuestions] ([pq_id],[pt_id],[pq_name],[pq_altname],[pq_description],[pq_hint],[pq_edit_style],[pq_show_style],[pq_search_style],[pq_required],[pq_priority],[pq_requires_approval],[pq_visible_male],[pq_visible_female],[pq_visible_couple]) VALUES (2,1,'Weight','Weight','','',5,2,0,1,2,0,1,1,1)
INSERT INTO [ProfileQuestions] ([pq_id],[pt_id],[pq_name],[pq_altname],[pq_description],[pq_hint],[pq_edit_style],[pq_show_style],[pq_search_style],[pq_required],[pq_priority],[pq_requires_approval],[pq_visible_male],[pq_visible_female],[pq_visible_couple]) VALUES (3,1,'Body Type','Body Type','','',5,2,2,0,3,0,1,1,1)
INSERT INTO [ProfileQuestions] ([pq_id],[pt_id],[pq_name],[pq_altname],[pq_description],[pq_hint],[pq_edit_style],[pq_show_style],[pq_search_style],[pq_required],[pq_priority],[pq_requires_approval],[pq_visible_male],[pq_visible_female],[pq_visible_couple]) VALUES (4,1,'Looking for','Looking for','','',5,2,2,0,4,0,1,1,1)
INSERT INTO [ProfileQuestions] ([pq_id],[pt_id],[pq_name],[pq_altname],[pq_description],[pq_hint],[pq_edit_style],[pq_show_style],[pq_search_style],[pq_required],[pq_priority],[pq_requires_approval],[pq_visible_male],[pq_visible_female],[pq_visible_couple]) VALUES (5,1,'Eyes','Eyes','','',5,2,2,0,5,0,1,1,1)
INSERT INTO [ProfileQuestions] ([pq_id],[pt_id],[pq_name],[pq_altname],[pq_description],[pq_hint],[pq_edit_style],[pq_show_style],[pq_search_style],[pq_required],[pq_priority],[pq_requires_approval],[pq_visible_male],[pq_visible_female],[pq_visible_couple]) VALUES (6,1,'Hair','Hair','','',5,2,2,0,6,0,1,1,1)
INSERT INTO [ProfileQuestions] ([pq_id],[pt_id],[pq_name],[pq_altname],[pq_description],[pq_hint],[pq_edit_style],[pq_show_style],[pq_search_style],[pq_required],[pq_priority],[pq_requires_approval],[pq_visible_male],[pq_visible_female],[pq_visible_couple]) VALUES (7,1,'Smoking','Smoking','','',5,2,2,0,7,0,1,1,1)
INSERT INTO [ProfileQuestions] ([pq_id],[pt_id],[pq_name],[pq_altname],[pq_description],[pq_hint],[pq_edit_style],[pq_show_style],[pq_search_style],[pq_required],[pq_priority],[pq_requires_approval],[pq_visible_male],[pq_visible_female],[pq_visible_couple]) VALUES (8,1,'Drinking','Drinking','','',5,2,2,0,8,0,1,1,1)
INSERT INTO [ProfileQuestions] ([pq_id],[pt_id],[pq_name],[pq_altname],[pq_description],[pq_hint],[pq_edit_style],[pq_show_style],[pq_search_style],[pq_required],[pq_priority],[pq_requires_approval],[pq_visible_male],[pq_visible_female],[pq_visible_couple]) VALUES (9,2,'Title','Title','Here''s your chance for good first impression. Write something original in 80 chars. That title shows right next to your photo and is the first thing other members will see.',' (max. 80 chars)',1,1,0,1,1,1,1,1,1)
INSERT INTO [ProfileQuestions] ([pq_id],[pt_id],[pq_name],[pq_altname],[pq_description],[pq_hint],[pq_edit_style],[pq_show_style],[pq_search_style],[pq_required],[pq_priority],[pq_requires_approval],[pq_visible_male],[pq_visible_female],[pq_visible_couple]) VALUES (10,2,'Tell us about you','About me','Now, tell the others what makes you so unique separates you from the rest. Don''t be shy... now is the time for bragging. Describe your perfect date, hobbies, interests, etc',' (max. 3333 chars)',2,4,0,1,2,1,1,1,1)
INSERT INTO [ProfileQuestions] ([pq_id],[pt_id],[pq_name],[pq_altname],[pq_description],[pq_hint],[pq_edit_style],[pq_show_style],[pq_search_style],[pq_required],[pq_priority],[pq_requires_approval],[pq_visible_male],[pq_visible_female],[pq_visible_couple]) VALUES (11,2,'What are you looking for?','Looking for','Tell the others exactly what are you looking for. Chat, flirt, dates, whatever...',' (max. 3333 chars)',2,4,0,1,3,1,1,1,1)
INSERT INTO [ProfileQuestions] ([pq_id],[pt_id],[pq_name],[pq_altname],[pq_description],[pq_hint],[pq_edit_style],[pq_show_style],[pq_search_style],[pq_required],[pq_priority],[pq_requires_approval],[pq_visible_male],[pq_visible_female],[pq_visible_couple]) VALUES (12,2,'Interests','Interests','','',6,5,0,0,4,0,1,1,1)
SET IDENTITY_INSERT [ProfileQuestions] OFF
ALTER TABLE [ProfileQuestions] CHECK CONSTRAINT ALL

PRINT 'INSERTING DATA INTO TABLE ProfileChoices'
ALTER TABLE [ProfileChoices] NOCHECK CONSTRAINT ALL
SET IDENTITY_INSERT [ProfileChoices] ON
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (1,1,'4'' 0" - 121cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (2,1,'4'' 1" - 124cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (3,1,'4'' 2" - 127cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (4,1,'4'' 3" - 130cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (5,1,'4'' 4" - 132cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (6,1,'4'' 5" - 135cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (7,1,'4'' 6" - 137cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (8,1,'4'' 7" - 140cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (9,1,'4'' 8" - 142cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (10,1,'4'' 9" - 145cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (11,1,'4'' 10" - 147cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (12,1,'4'' 11" - 150cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (13,1,'5'' 0" - 152cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (14,1,'5'' 1" - 155cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (15,1,'5'' 2" - 157cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (16,1,'5'' 3" - 160cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (17,1,'5'' 4" - 163cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (18,1,'5'' 5" - 165cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (19,1,'5'' 6" - 168cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (20,1,'5'' 7" - 170cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (21,1,'5'' 8" - 173cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (22,1,'5'' 9" - 175cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (23,1,'5'' 10" - 178cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (24,1,'5'' 11" - 180cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (25,1,'6'' 0" - 183cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (26,1,'6'' 1" - 185cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (27,1,'6'' 2" - 188cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (28,1,'6'' 3" - 190cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (29,1,'6'' 4" - 193cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (30,1,'6'' 5" - 196cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (31,1,'6'' 6" - 198cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (32,1,'6'' 7" - 201cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (33,1,'6'' 8" - 203cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (34,1,'6'' 9" - 206cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (35,1,'6'' 10" - 208cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (36,1,'6'' 11" - 211cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (37,1,'7'' 0" - 213cm')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (38,2,'88 lbs - 40 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (39,2,'90 lbs - 41 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (40,2,'93 lbs - 42 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (41,2,'95 lbs - 43 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (42,2,'97 lbs - 44 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (43,2,'99 lbs - 45 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (44,2,'101 lbs - 46 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (45,2,'103 lbs - 47 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (46,2,'105 lbs - 48 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (47,2,'108 lbs - 49 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (48,2,'110 lbs - 50 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (49,2,'112 lbs - 51 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (50,2,'115 lbs - 52 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (51,2,'117 lbs - 53 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (52,2,'119 lbs - 54 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (53,2,'121 lbs - 55 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (54,2,'123 lbs - 56 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (55,2,'125 lbs - 57 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (56,2,'128 lbs - 58 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (57,2,'130 lbs - 59 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (58,2,'132 lbs - 60 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (59,2,'134 lbs - 61 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (60,2,'137 lbs - 62 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (61,2,'139 lbs - 63 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (62,2,'141 lbs - 64 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (63,2,'143 lbs - 65 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (64,2,'146 lbs - 66 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (65,2,'148 lbs - 67 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (66,2,'150 lbs - 68 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (67,2,'152 lbs - 69 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (68,2,'154 lbs - 70 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (69,2,'156 lbs - 71 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (70,2,'158 lbs - 72 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (71,2,'161 lbs - 73 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (72,2,'163 lbs - 74 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (73,2,'165 lbs - 75 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (74,2,'167 lbs - 76 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (75,2,'169 lbs - 77 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (76,2,'171 lbs - 78 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (77,2,'174 lbs - 79 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (78,2,'176 lbs - 80 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (79,2,'178 lbs - 81 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (80,2,'180 lbs - 82 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (81,2,'183 lbs - 83 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (82,2,'185 lbs - 84 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (83,2,'187 lbs - 85 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (84,2,'189 lbs - 86 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (85,2,'191 lbs - 87 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (86,2,'194 lbs - 88 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (87,2,'196 lbs - 89 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (88,2,'198 lbs - 90 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (89,2,'200 lbs - 91 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (90,2,'202 lbs - 92 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (91,2,'205 lbs - 93 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (92,2,'207 lbs - 94 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (93,2,'209 lbs - 95 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (94,2,'211 lbs - 96 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (95,2,'213 lbs - 97 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (96,2,'216 lbs - 98 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (97,2,'218 lbs - 99 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (98,2,'220 lbs - 100 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (99,2,'222 lbs - 101 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (100,2,'224 lbs - 102 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (101,2,'227 lbs - 103 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (102,2,'229 lbs - 104 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (103,2,'231 lbs - 105 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (104,2,'233 lbs - 106 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (105,2,'235 lbs - 107 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (106,2,'238 lbs - 108 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (107,2,'240 lbs - 109 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (108,2,'242 lbs - 110 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (109,2,'244 lbs - 111 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (110,2,'246 lbs - 112 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (111,2,'249 lbs - 113 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (112,2,'251 lbs - 114 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (113,2,'253 lbs - 115 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (114,2,'255 lbs - 116 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (115,2,'258 lbs - 117 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (116,2,'260 lbs - 118 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (117,2,'262 lbs - 119 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (118,2,'265 lbs - 120 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (119,2,'267 lbs - 121 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (120,2,'269 lbs - 122 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (121,2,'271 lbs - 123 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (122,2,'273 lbs - 124 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (123,2,'275 lbs - 125 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (124,2,'277 lbs - 126 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (125,2,'279 lbs - 127 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (126,2,'282 lbs - 128 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (127,2,'284 lbs - 129 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (128,2,'286 lbs - 130 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (129,2,'288 lbs - 131 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (130,2,'291 lbs - 132 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (131,2,'293 lbs - 133 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (132,2,'295 lbs - 134 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (133,2,'297 lbs - 135 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (134,2,'300 lbs - 136 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (135,2,'302 lbs - 137 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (136,2,'304 lbs - 138 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (137,2,'306 lbs - 139 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (138,2,'308 lbs - 140 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (139,2,'> 308 lbs - 140 kg')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (140,3,'Slim')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (141,3,'Athletic')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (142,3,'Normal')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (143,3,'Few extra pounds')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (144,4,'Chat')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (145,4,'Friendship')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (146,4,'Dates')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (147,4,'Something serious')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (148,4,'Sex')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (149,5,'Blue')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (150,5,'Green')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (151,5,'Hazel')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (152,5,'Brown')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (153,5,'Gray')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (154,5,'Black')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (155,5,'Blue-green')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (156,5,'Other')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (157,6,'Brown')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (158,6,'Blond')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (159,6,'Black')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (160,6,'Gray')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (161,6,'White')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (162,6,'Red')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (163,6,'Bald')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (164,7,'No')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (165,7,'Sometimes')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (166,7,'Often')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (167,7,'Smoker')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (168,7,'Total addict')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (169,8,'No')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (170,8,'Often')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (171,8,'Only in company')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (172,8,'Daily')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (173,8,'Alcoholic')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (174,12,'Computers')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (175,12,'Sport')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (176,12,'Music')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (177,12,'Movies')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (178,12,'Nature')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (179,12,'Chat')
INSERT INTO [ProfileChoices] ([pc_id],[pq_id],[pc_value]) VALUES (180,12,'Adventures')
SET IDENTITY_INSERT [ProfileChoices] OFF
ALTER TABLE [ProfileChoices] CHECK CONSTRAINT ALL
