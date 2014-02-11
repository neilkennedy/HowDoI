--http://www.youtube.com/watch?v=f0YJFp7BQyc


/*View Polygon in Spatial Map */

SELECT geography ::STPolyFromText( 'POLYGON((174.6596469283104 -36.724981172899945, 174.6591963171959 -36.72647744053394, 174.6586598753929 -36.72787049109019, 174.65816634893417 -36.72903994611866, 174.66164249181747 -36.72924631868662, 174.66250079870224 -36.72909153931266, 174.6627797484398 -36.72658063033066, 174.6617712378502 -36.72587549728961, 174.66084855794907 -36.72544553494063, 174.66059106588364 -36.724981172899945, 174.6596469283104 -36.724981172899945))', 4326);


/* View Route from Home to Court */

SELECT  geography::STLineFromText ('LINESTRING(174.6621197462082 -36.726017362965905, 174.662806391716 -36.726567710243245, 
174.6646946668625 -36.726567710243245, 174.66533839702606 -36.726533313653945,
174.66666877269745 -36.72698046811294, 174.66842830181122 -36.727118053576504,
174.67001616954803 -36.72732443130964, 174.67134654521942 -36.72770278904656,
174.67130362987518 -36.726739692958574, 174.67207610607147 -36.72701486450194,
174.67327773571014 -36.725879775530316, 174.67370688915253 -36.7244350925741,
174.6742218732834 -36.72481346454656, 174.6749085187912 -36.72519183465482,
174.67533767223358 -36.7257077908892, 174.67572391033173 -36.72643012379366,
174.67688262462616 -36.72677408945542, 174.6776121854782 -36.72677408945542,
174.67709720134735 -36.72481346454656, 174.6763676404953 -36.724056718737515,
174.6781700849533 -36.71896568948905, 174.6772688627243 -36.71796808340889,
174.67735469341278 -36.71758967771348, 174.68010127544403 -36.71968808580415,
174.68181788921356 -36.721820835286664, 174.68396365642548 -36.722508806358036,
174.68962848186493 -36.72133925187012, 174.69084117097913 -36.72168324033291,
174.70148417635022 -36.729628944985876, 174.70654818697034 -36.73279323913776,
174.70914194290705 -36.73398464343141, 174.71240350906916 -36.73456932661122, 174.7152359217889 -36.735050827062864, 174.71665212814875 -36.73577307207795, 174.71806833450862 -36.734122216344915, 174.71914121811457 -36.738042940745885, 174.72124406998225 -36.74400446553403, 174.72502062027522 -36.74964401657368, 174.7299710623323 -36.75460872607822, 174.7364512793123 -36.75997244530984, 174.73885453858964 -36.763238629044366, 174.74224485078446 -36.77047736211463, 174.74593557038895 -36.77693994767233, 174.75303869347837 -36.78813853883044, 174.7550986300018 -36.791747197130874, 174.76084928612974 -36.798076862529356, 174.7617075930145 -36.80058543179314, 174.76020555596617 -36.80501818207179, 174.7591326723602 -36.80635826536489, 174.7516224871185 -36.81448889471376, 174.75020628075865 -36.81940181099485, 174.75046377282408 -36.823443525119686, 174.7463438997772 -36.82997034353399, 174.74166612725523 -36.83706123935265, 174.74205236535337 -36.838641186262564, 174.74475603204039 -36.84035848284198, 174.7463438997772 -36.842213119835534, 174.74951963525083 -36.84310607716101, 174.7525237093475 -36.8449719362233, 174.7530816088226 -36.84644869134304, 174.75333910088804 -36.849058700404896, 174.75411157708433 -36.8510161487294, 174.7567723284271 -36.854038075793014, 174.7567723284271 -36.855927444568664, 174.75595693688658 -36.85761001513055, 174.75685815911558 -36.859738928878585, 174.75896101098326 -36.86004795981614, 174.76054887872007 -36.85991061288706, 174.76325254540708 -36.86087203620694, 174.7661385438765 -36.859979286382455, 174.76794098833452 -36.85695759420739, 174.77081631639848 -36.854656907559146,
174.77287625292192 -36.85338634943034, 174.77360581377397 -36.85232181149516 174.77334832170854 -36.850948192253924, 174.77270459154497 -36.85036439660239
174.77203403929127 -36.849930839593966
)', 4326)


/*Calculate the area of Kim Dot Coms Property in Sqaure feet*/

DECLARE @Plot geography;
SET @Plot = Geography::STPolyFromText ('POLYGON((174.6596469283104 -36.724981172899945, 174.6591963171959 -36.72647744053394, 174.6586598753929 -36.72787049109019, 174.65816634893417 -36.72903994611866, 174.66164249181747 -36.72924631868662, 174.66250079870224 -36.72909153931266, 174.6627797484398 -36.72658063033066, 174.6617712378502 -36.72587549728961, 174.66084855794907 -36.72544553494063, 174.66059106588364 -36.724981172899945, 174.6596469283104 -36.724981172899945))',4157);

SELECT @Plot.STArea () AS AREA_SQR_FEET


/*Calculate the area of Kim Dot Coms Property in Square Metres*/

DECLARE @Plot geography;
SET @Plot = Geography::STPolyFromText ('POLYGON((174.6596469283104 -36.724981172899945, 174.6591963171959 -36.72647744053394, 174.6586598753929 -36.72787049109019, 174.65816634893417 -36.72903994611866, 174.66164249181747 -36.72924631868662, 174.66250079870224 -36.72909153931266, 174.6627797484398 -36.72658063033066, 174.6617712378502 -36.72587549728961, 174.66084855794907 -36.72544553494063, 174.66059106588364 -36.724981172899945, 174.6596469283104 -36.724981172899945))',4326);

SELECT @Plot.STArea () AS AREA_SQR_Metres


/*Calculate the Length of the perimeter of Kim Dot Coms Property in  Metres*/

DECLARE @Perimeter geography;
SET @Perimeter = Geography::STPolyFromText ('POLYGON((174.6596469283104 -36.724981172899945, 174.6591963171959 -36.72647744053394, 174.6586598753929 -36.72787049109019, 174.65816634893417 -36.72903994611866, 174.66164249181747 -36.72924631868662, 174.66250079870224 -36.72909153931266, 174.6627797484398 -36.72658063033066, 174.6617712378502 -36.72587549728961, 174.66084855794907 -36.72544553494063, 174.66059106588364 -36.724981172899945, 174.6596469283104 -36.724981172899945))',4326);

SELECT @Perimeter.STLength () AS Perimeter_Metres




/* Length of road route from home to Court */

DECLARE @Home2CourtRoute Geography;
SET @Home2CourtRoute = Geography ::STLineFromText ('LINESTRING(174.6621197462082 -36.726017362965905, 174.662806391716 -36.726567710243245, 
174.6646946668625 -36.726567710243245, 174.66533839702606 -36.726533313653945,
174.66666877269745 -36.72698046811294, 174.66842830181122 -36.727118053576504,
174.67001616954803 -36.72732443130964, 174.67134654521942 -36.72770278904656,
174.67130362987518 -36.726739692958574, 174.67207610607147 -36.72701486450194,
174.67327773571014 -36.725879775530316, 174.67370688915253 -36.7244350925741,
174.6742218732834 -36.72481346454656, 174.6749085187912 -36.72519183465482,
174.67533767223358 -36.7257077908892, 174.67572391033173 -36.72643012379366,
174.67688262462616 -36.72677408945542, 174.6776121854782 -36.72677408945542,
174.67709720134735 -36.72481346454656, 174.6763676404953 -36.724056718737515,
174.6781700849533 -36.71896568948905, 174.6772688627243 -36.71796808340889,
174.67735469341278 -36.71758967771348, 174.68010127544403 -36.71968808580415,
174.68181788921356 -36.721820835286664, 174.68396365642548 -36.722508806358036,
174.68962848186493 -36.72133925187012, 174.69084117097913 -36.72168324033291,
174.70148417635022 -36.729628944985876, 174.70654818697034 -36.73279323913776,
174.70914194290705 -36.73398464343141, 174.71240350906916 -36.73456932661122, 174.7152359217889 -36.735050827062864, 174.71665212814875 -36.73577307207795, 174.71806833450862 -36.734122216344915, 174.71914121811457 -36.738042940745885, 174.72124406998225 -36.74400446553403, 174.72502062027522 -36.74964401657368, 174.7299710623323 -36.75460872607822, 174.7364512793123 -36.75997244530984, 174.73885453858964 -36.763238629044366, 174.74224485078446 -36.77047736211463, 174.74593557038895 -36.77693994767233, 174.75303869347837 -36.78813853883044, 174.7550986300018 -36.791747197130874, 174.76084928612974 -36.798076862529356, 174.7617075930145 -36.80058543179314, 174.76020555596617 -36.80501818207179, 174.7591326723602 -36.80635826536489, 174.7516224871185 -36.81448889471376, 174.75020628075865 -36.81940181099485, 174.75046377282408 -36.823443525119686, 174.7463438997772 -36.82997034353399, 174.74166612725523 -36.83706123935265, 174.74205236535337 -36.838641186262564, 174.74475603204039 -36.84035848284198, 174.7463438997772 -36.842213119835534, 174.74951963525083 -36.84310607716101, 174.7525237093475 -36.8449719362233, 174.7530816088226 -36.84644869134304, 174.75333910088804 -36.849058700404896, 174.75411157708433 -36.8510161487294, 174.7567723284271 -36.854038075793014, 174.7567723284271 -36.855927444568664, 174.75595693688658 -36.85761001513055, 174.75685815911558 -36.859738928878585, 174.75896101098326 -36.86004795981614, 174.76054887872007 -36.85991061288706, 174.76325254540708 -36.86087203620694, 174.7661385438765 -36.859979286382455, 174.76794098833452 -36.85695759420739, 174.77081631639848 -36.854656907559146,
174.77287625292192 -36.85338634943034, 174.77360581377397 -36.85232181149516 174.77334832170854 -36.850948192253924, 174.77270459154497 -36.85036439660239
174.77203403929127 -36.849930839593966
)', 4326)

SELECT @Home2CourtRoute.STLength() As LengthofRoute



/* Calculate the straightline distance via Helicopter*/

DECLARE @Helicopter_route geography;
SET @Helicopter_route = geography::STLineFromText ('Linestring (174.6621197462082 -36.726017362965905 ,174.77203403929127 -36.849930839593966)', 4326);
SELECT @Helicopter_route.STLength() As LengthofRoute


/* Create a database called "Spatial_Demo */

CREATE DATABASE Spatial_demo

/* Create Route Table */

USE [Spatial_demo]
GO

CREATE TABLE [dbo].[Route](
[Timestamp] [varchar](16) NULL,
[Longitude] [real] NULL,
[Latitude ] [real] NULL
) ON [PRIMARY]

GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'04 01 2013 10 49', 174.7032, -36.7297974)
GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'05 01 2013 11 15', 174.663559, -36.72653)
GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'06 01 2013 11 41', 174.670746, -36.72768)
GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'07 01 2013 12 07', 174.6801, -36.71965)
GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'08 01 2013 12 33', 174.688171, -36.7216949)
GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'09 01 2013 12 59', 174.7003, -36.72868)
GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'10 01 2013 13 25', 174.715118, -36.73501)
GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'11 01 2013 13 51', 174.721481, -36.7443771)
GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'12 01 2013 14 17', 174.735947, -36.7595444)
GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'13 01 2013 14 43', 174.74469, -36.7745132)
GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'14 01 2013 15 09', 174.755447, -36.7919922)
GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'15 01 2013 15 35', 174.756, -36.8098259)
GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'16 01 2013 16 01', 174.746475, -36.8296165)
GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'17 01 2013 16 27', 174.749374, -36.8430443)
GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'18 01 2013 16 53', 174.756241, -36.85678)
GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'19 01 2013 17 19', 174.7689, -36.85613)
GO
INSERT [dbo].[Route] ([Timestamp], [Longitude], [Latitude ]) VALUES (N'20 01 2013 17 45', 174.772629, -36.8504639)
GO


/* Did the transit stay within +/- 20 m of the linestring representation of the journey */


/* Define the road with a line string & Longitudes and Latitudes */
DECLARE @Test_road geography;
SET @Test_road = geography::STLineFromText ('LINESTRING(174.6621197462082 -36.726017362965905, 174.662806391716 -36.726567710243245, 
174.6646946668625 -36.726567710243245, 174.66533839702606 -36.726533313653945,
174.66666877269745 -36.72698046811294, 174.66842830181122 -36.727118053576504,
174.67001616954803 -36.72732443130964, 174.67134654521942 -36.72770278904656,
174.67130362987518 -36.726739692958574, 174.67207610607147 -36.72701486450194,
174.67327773571014 -36.725879775530316, 174.67370688915253 -36.7244350925741,
174.6742218732834 -36.72481346454656, 174.6749085187912 -36.72519183465482,
174.67533767223358 -36.7257077908892, 174.67572391033173 -36.72643012379366,
174.67688262462616 -36.72677408945542, 174.6776121854782 -36.72677408945542,
174.67709720134735 -36.72481346454656, 174.6763676404953 -36.724056718737515,
174.6781700849533 -36.71896568948905, 174.6772688627243 -36.71796808340889,
174.67735469341278 -36.71758967771348, 174.68010127544403 -36.71968808580415,
174.68181788921356 -36.721820835286664, 174.68396365642548 -36.722508806358036,
174.68962848186493 -36.72133925187012, 174.69084117097913 -36.72168324033291,
174.70148417635022 -36.729628944985876, 174.70654818697034 -36.73279323913776,
174.70914194290705 -36.73398464343141, 174.71240350906916 -36.73456932661122, 174.7152359217889 -36.735050827062864, 174.71665212814875 -36.73577307207795, 174.71806833450862 -36.734122216344915, 174.71914121811457 -36.738042940745885, 174.72124406998225 -36.74400446553403, 174.72502062027522 -36.74964401657368, 174.7299710623323 -36.75460872607822, 174.7364512793123 -36.75997244530984, 174.73885453858964 -36.763238629044366, 174.74224485078446 -36.77047736211463, 174.74593557038895 -36.77693994767233, 174.75303869347837 -36.78813853883044, 174.7550986300018 -36.791747197130874, 174.76084928612974 -36.798076862529356, 174.7617075930145 -36.80058543179314, 174.76020555596617 -36.80501818207179, 174.7591326723602 -36.80635826536489, 174.7516224871185 -36.81448889471376, 174.75020628075865 -36.81940181099485, 174.75046377282408 -36.823443525119686, 174.7463438997772 -36.82997034353399, 174.74166612725523 -36.83706123935265, 174.74205236535337 -36.838641186262564, 174.74475603204039 -36.84035848284198, 174.7463438997772 -36.842213119835534, 174.74951963525083 -36.84310607716101, 174.7525237093475 -36.8449719362233, 174.7530816088226 -36.84644869134304, 174.75333910088804 -36.849058700404896, 174.75411157708433 -36.8510161487294, 174.7567723284271 -36.854038075793014, 174.7567723284271 -36.855927444568664, 174.75595693688658 -36.85761001513055, 174.75685815911558 -36.859738928878585, 174.75896101098326 -36.86004795981614, 174.76054887872007 -36.85991061288706, 174.76325254540708 -36.86087203620694, 174.7661385438765 -36.859979286382455, 174.76794098833452 -36.85695759420739, 174.77081631639848 -36.854656907559146,
174.77287625292192 -36.85338634943034, 174.77360581377397 -36.85232181149516 174.77334832170854 -36.850948192253924, 174.77270459154497 -36.85036439660239
174.77203403929127 -36.849930839593966)', 4326);

/* Add 20m buffer to each side of the line string */
DECLARE @Routebuffer geography;
SET @Routebuffer = @Test_road.STBuffer ('20');

Select *,
  @Routebuffer. STContains(geography ::Point( latitude, longitude, 4326 )) As InRouteBuffer
From dbo.Route


/* What is the total of the area in which the home detainee can roam, including  his home and  the 2 km around the courthouse */

/*  Area of Circle */

DECLARE @Location_of_court geography;
SET @Location_of_court = geography::STGeomFromText ('POINT(174.77203403929127 -36.849930839593966)', 4326);

/* Add 2000m buffer to the point */
DECLARE @Buffered_Point geography;
SET @Buffered_Point = @Location_of_court.STBuffer ('2000');

SELECT @Buffered_Point.STArea () AS AREA_around_court

/* Area of kims property */

DECLARE @Plot geography;
SET @Plot = Geography::STPolyFromText ('POLYGON((174.6596469283104 -36.724981172899945, 174.6591963171959 -36.72647744053394, 174.6586598753929 -36.72787049109019, 174.65816634893417 -36.72903994611866, 174.66164249181747 -36.72924631868662, 174.66250079870224 -36.72909153931266, 174.6627797484398 -36.72658063033066, 174.6617712378502 -36.72587549728961, 174.66084855794907 -36.72544553494063, 174.66059106588364 -36.724981172899945, 174.6596469283104 -36.724981172899945))',4326);

SELECT @Plot.STArea () AS AREA_SQR_Metres


DECLARE @Total_area 
geography = @Buffered_Point.STUnion(@Plot);
SELECT @Total_area.STArea () AS total;