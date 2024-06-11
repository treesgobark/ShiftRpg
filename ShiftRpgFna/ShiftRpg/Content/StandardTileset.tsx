<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.10" tiledversion="1.10.2" name="TiledIcons" tilewidth="16" tileheight="16" tilecount="1024" columns="32">
 <image source="StandardTilesetIcons.png" width="512" height="512"/>
 <tile id="0" type="SolidCollision"/>
 <tile id="1" type="SolidCollision"/>
 <tile id="2" type="SolidCollision"/>
 <tile id="3" type="CloudCollision"/>
 <tile id="4" type="CloudCollision"/>
 <tile id="5" type="CloudCollision"/>
 <tile id="6" type="OneWayCollision"/>
 <tile id="32" type="Water"/>
 <tile id="33" type="BreakableCollision"/>
 <tile id="34" type="IceCollision"/>
 <tile id="64" type="Door"/>
 <tile id="96" type="Ladder"/>
 <tile id="256">
  <properties>
   <property name="MatchType" value="Empty"/>
  </properties>
 </tile>
 <tile id="257">
  <properties>
   <property name="MatchType" value="Ignore"/>
  </properties>
 </tile>
 <tile id="258">
  <properties>
   <property name="MatchType" value="NonEmpty"/>
  </properties>
 </tile>
 <tile id="259">
  <properties>
   <property name="MatchType" value="Other"/>
  </properties>
 </tile>
 <tile id="260">
  <properties>
   <property name="MatchType" value="Negate"/>
  </properties>
 </tile>
 <wangsets>
  <wangset name="CollisionSet" type="mixed" tile="-1">
   <wangcolor name="SolidCollision" color="#ff0000" tile="-1" probability="1"/>
   <wangtile tileid="0" wangid="1,1,1,1,1,1,1,1"/>
   <wangtile tileid="73" wangid="1,1,0,0,0,1,1,1"/>
   <wangtile tileid="74" wangid="1,1,1,1,0,0,0,1"/>
   <wangtile tileid="75" wangid="0,0,0,1,0,0,0,0"/>
   <wangtile tileid="76" wangid="0,0,0,1,1,1,0,0"/>
   <wangtile tileid="77" wangid="0,0,0,0,0,1,0,0"/>
   <wangtile tileid="105" wangid="0,0,0,1,1,1,1,1"/>
   <wangtile tileid="106" wangid="0,1,1,1,1,1,0,0"/>
   <wangtile tileid="107" wangid="0,1,1,1,0,0,0,0"/>
   <wangtile tileid="109" wangid="0,0,0,0,0,1,1,1"/>
   <wangtile tileid="139" wangid="0,1,0,0,0,0,0,0"/>
   <wangtile tileid="140" wangid="1,1,0,0,0,0,0,1"/>
   <wangtile tileid="141" wangid="0,0,0,0,0,0,0,1"/>
  </wangset>
  <wangset name="Bricks" type="mixed" tile="-1">
   <wangcolor name="Walls" color="#00ff00" tile="-1" probability="1"/>
   <wangcolor name="Platforms" color="#aa0000" tile="-1" probability="1"/>
   <wangcolor name="Darkness" color="#0000ff" tile="-1" probability="1"/>
   <wangtile tileid="288" wangid="0,0,0,2,0,0,0,0"/>
   <wangtile tileid="289" wangid="0,0,0,2,0,2,0,0"/>
   <wangtile tileid="290" wangid="0,0,0,0,0,2,0,0"/>
   <wangtile tileid="294" wangid="0,0,1,0,1,0,0,0"/>
   <wangtile tileid="295" wangid="0,0,1,0,1,0,1,0"/>
   <wangtile tileid="296" wangid="0,0,0,0,1,0,1,0"/>
   <wangtile tileid="320" wangid="0,2,0,2,0,0,0,0"/>
   <wangtile tileid="321" wangid="0,2,0,2,0,2,0,2"/>
   <wangtile tileid="322" wangid="0,0,0,0,0,2,0,2"/>
   <wangtile tileid="326" wangid="1,0,1,0,1,0,0,0"/>
   <wangtile tileid="327" wangid="1,0,1,0,1,0,1,0"/>
   <wangtile tileid="328" wangid="1,0,0,0,1,0,1,0"/>
   <wangtile tileid="352" wangid="0,2,0,0,0,0,0,0"/>
   <wangtile tileid="353" wangid="0,2,0,0,0,0,0,2"/>
   <wangtile tileid="354" wangid="0,0,0,0,0,0,0,2"/>
   <wangtile tileid="358" wangid="1,0,1,0,0,0,0,0"/>
   <wangtile tileid="359" wangid="1,0,1,0,0,0,1,0"/>
   <wangtile tileid="360" wangid="1,0,0,0,0,0,1,0"/>
   <wangtile tileid="361" wangid="0,0,0,0,1,0,0,0"/>
   <wangtile tileid="384" wangid="0,2,0,3,0,2,0,2"/>
   <wangtile tileid="385" wangid="0,2,0,3,0,3,0,2"/>
   <wangtile tileid="386" wangid="0,2,0,2,0,3,0,2"/>
   <wangtile tileid="392" wangid="0,0,1,0,0,0,0,0"/>
   <wangtile tileid="393" wangid="1,0,1,0,1,0,1,0"/>
   <wangtile tileid="394" wangid="0,0,0,0,0,0,1,0"/>
   <wangtile tileid="416" wangid="0,3,0,3,0,2,0,2"/>
   <wangtile tileid="417" wangid="0,3,0,3,0,3,0,3"/>
   <wangtile tileid="418" wangid="0,2,0,2,0,3,0,3"/>
   <wangtile tileid="424" wangid="0,0,1,0,0,0,1,0"/>
   <wangtile tileid="425" wangid="1,0,0,0,0,0,0,0"/>
   <wangtile tileid="426" wangid="1,0,0,0,1,0,0,0"/>
   <wangtile tileid="448" wangid="0,3,0,2,0,2,0,2"/>
   <wangtile tileid="449" wangid="0,3,0,2,0,2,0,3"/>
   <wangtile tileid="450" wangid="0,2,0,2,0,2,0,3"/>
   <wangtile tileid="480" wangid="0,3,0,2,0,3,0,3"/>
   <wangtile tileid="481" wangid="0,3,0,2,0,2,0,3"/>
   <wangtile tileid="482" wangid="0,3,0,3,0,2,0,3"/>
   <wangtile tileid="512" wangid="0,2,0,2,0,3,0,3"/>
   <wangtile tileid="513" wangid="0,2,0,2,0,2,0,2"/>
   <wangtile tileid="514" wangid="0,3,0,3,0,2,0,2"/>
   <wangtile tileid="521" wangid="0,2,0,0,0,2,0,2"/>
   <wangtile tileid="522" wangid="0,2,0,2,0,0,0,2"/>
   <wangtile tileid="544" wangid="0,2,0,3,0,3,0,3"/>
   <wangtile tileid="545" wangid="0,2,0,3,0,3,0,2"/>
   <wangtile tileid="546" wangid="0,3,0,3,0,3,0,2"/>
   <wangtile tileid="553" wangid="0,0,0,2,0,2,0,2"/>
   <wangtile tileid="554" wangid="0,2,0,2,0,2,0,0"/>
  </wangset>
  <wangset name="Blob" type="mixed" tile="-1">
   <wangcolor name="Big Bricks" color="#ff0000" tile="-1" probability="1"/>
   <wangcolor name="Small Bricks" color="#00ff00" tile="-1" probability="1"/>
   <wangtile tileid="299" wangid="1,1,2,2,2,1,1,1"/>
   <wangtile tileid="300" wangid="1,1,2,2,2,2,2,1"/>
   <wangtile tileid="301" wangid="1,1,1,1,2,2,2,1"/>
   <wangtile tileid="302" wangid="1,1,1,1,2,1,1,1"/>
   <wangtile tileid="303" wangid="2,2,2,1,2,2,2,2"/>
   <wangtile tileid="304" wangid="2,2,2,1,2,1,2,2"/>
   <wangtile tileid="305" wangid="2,2,2,2,2,1,2,2"/>
   <wangtile tileid="331" wangid="2,2,2,2,2,1,1,1"/>
   <wangtile tileid="332" wangid="2,2,2,2,2,2,2,2"/>
   <wangtile tileid="333" wangid="2,1,1,1,2,2,2,2"/>
   <wangtile tileid="334" wangid="2,1,1,1,2,1,1,1"/>
   <wangtile tileid="335" wangid="2,1,2,1,2,2,2,2"/>
   <wangtile tileid="336" wangid="2,1,2,1,2,1,2,1"/>
   <wangtile tileid="337" wangid="2,2,2,2,2,1,2,1"/>
   <wangtile tileid="363" wangid="2,2,2,1,1,1,1,1"/>
   <wangtile tileid="364" wangid="2,2,2,1,1,1,2,2"/>
   <wangtile tileid="365" wangid="2,1,1,1,1,1,2,2"/>
   <wangtile tileid="366" wangid="2,1,1,1,1,1,1,1"/>
   <wangtile tileid="367" wangid="2,1,2,2,2,2,2,2"/>
   <wangtile tileid="368" wangid="2,1,2,2,2,2,2,1"/>
   <wangtile tileid="369" wangid="2,2,2,2,2,2,2,1"/>
   <wangtile tileid="395" wangid="1,1,2,1,1,1,1,1"/>
   <wangtile tileid="396" wangid="1,1,2,1,1,1,2,1"/>
   <wangtile tileid="397" wangid="1,1,1,1,1,1,2,1"/>
   <wangtile tileid="399" wangid="1,1,2,1,2,2,2,1"/>
   <wangtile tileid="400" wangid="1,1,2,1,2,1,2,1"/>
   <wangtile tileid="401" wangid="1,1,2,2,2,1,2,1"/>
   <wangtile tileid="420" wangid="1,1,1,1,1,1,1,1"/>
   <wangtile tileid="427" wangid="2,2,2,1,2,1,1,1"/>
   <wangtile tileid="428" wangid="2,1,1,1,2,1,2,2"/>
   <wangtile tileid="429" wangid="2,2,2,1,2,2,2,1"/>
   <wangtile tileid="430" wangid="2,1,2,2,2,1,2,2"/>
   <wangtile tileid="431" wangid="2,1,2,1,1,1,2,2"/>
   <wangtile tileid="432" wangid="2,1,2,1,1,1,2,1"/>
   <wangtile tileid="433" wangid="2,2,2,1,1,1,2,1"/>
   <wangtile tileid="459" wangid="2,1,2,1,2,1,1,1"/>
   <wangtile tileid="460" wangid="2,1,1,1,2,1,2,1"/>
   <wangtile tileid="461" wangid="1,1,2,1,2,1,1,1"/>
   <wangtile tileid="462" wangid="1,1,1,1,2,1,2,1"/>
   <wangtile tileid="463" wangid="2,1,2,1,2,1,2,2"/>
   <wangtile tileid="464" wangid="2,2,2,1,2,1,2,1"/>
   <wangtile tileid="491" wangid="2,1,2,2,2,1,1,1"/>
   <wangtile tileid="492" wangid="2,1,1,1,2,2,2,1"/>
   <wangtile tileid="493" wangid="2,1,2,1,1,1,1,1"/>
   <wangtile tileid="494" wangid="2,1,1,1,1,1,2,1"/>
   <wangtile tileid="495" wangid="2,1,2,1,2,2,2,1"/>
   <wangtile tileid="496" wangid="2,1,2,2,2,1,2,1"/>
  </wangset>
 </wangsets>
</tileset>
