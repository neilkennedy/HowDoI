select count(1) as count
from Route
where geography::Point([Latitude],[Longitude], 4326).Filter(geography::Parse('POLYGON((174.65635299682617 -36.701801605401094,174.7118854522705 -36.701801605401094,174.7118854522705 -36.73547933353356,174.65635299682617 -36.73547933353356,174.65635299682617 -36.701801605401094))')) = 0


select geography::Parse('POLYGON((174.47834014892578 -36.654097781310135,174.92259979248047 -36.654097781310135,174.92259979248047 -36.923273215398766,174.47834014892578 -36.923273215398766,174.47834014892578 -36.654097781310135))')

select * from Route



--http://sqlspatialtools.codeplex.com/discussions/41166

DECLARE @resolution float = 500; -- meters
DECLARE @dist float = 45;

SELECT ROUND([longitude] / @resolution, 0) * @resolution as cluster_x, ROUND([latitude] / @resolution, 0) * @resolution as cluster_y, COUNT(*) cluster_size, geography::Point(cluster_x, cluster_y, 0) as cluster_point

FROM route

WHERE geography::Point([Latitude],[Longitude], 4326).STDistance(geography::Parse('POINT(174.68176 -36.72306)')) < @dist

GROUP BY cluster_x, cluster_y

