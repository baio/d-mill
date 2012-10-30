united states president elections 2012, countries mention count.

source:
http://www.npr.org/2012/10/03/162258551/transcript-first-obama-romney-presidential-debate
http://www.npr.org/2012/10/16/163050988/transcript-obama-romney-2nd-presidential-debate
http://www.npr.org/2012/10/22/163436694/transcript-3rd-obama-romney-presidential-debate

Prepare data
read uspe-1,2,3
1. Trim \s+ -> \s
2. Remove [^a-zA-Z0-9 -:]
3. To lower case
create uspe-all.txt
----

1. Loop countries, including alias (country list -  mongodb://<dbuser>:<dbpassword>@ds037837.mongolab.com:37837/dchamber)
2. check stop-words list
3. get country stem and iter word stem
4. compare stems
5. if true store - word index and country name
6. wite file country name;word position
