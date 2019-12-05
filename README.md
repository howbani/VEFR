# Ad-Hoc Vehicular Networks (VANETs) Simulator 

VEFR (Vehicular Environment Fuzzy Router) 
----

To simulate VEFR, a new platform with a graphical user interface that provides a detailed visualization of simulation runs with lots of relevant pieces of in-formation is developed.

Developed by Ammar Hawbani et al.  (anmande@ustc.edu.cn), Copyright © 2019 Ammar Hawbani et al. 

# Cite this work:
A. Hawbani, E. Torbosh, W. Xingfu, P. Sincak, L. Zhao and A. Y. Al-Dubai, "Fuzzy based Distributed Protocol for Vehicle to Vehicle Communication," in IEEE Transactions on Fuzzy Systems. doi: 10.1109/TFUZZ.2019.2957254
#  Download  this work: https://ieeexplore.ieee.org/document/8920135

Implementation 
-----
Crisps, fuzzy sets and fuzzy membership functions are implemented in:
[https://github.com/howbani/VEFR/tree/master/FuzzySets] 
	
Inter-path is implemented on
[https://github.com/howbani/VEFR/blob/master/Routing/InterRouting.cs]
	
Intra-path is selected on 
[https://github.com/howbani/VEFR/blob/master/Routing/IntraRouting.cs]
	
Road network including lanes, segment, junction are implemented in the link 
[https://github.com/howbani/VEFR/tree/master/RoadNet/Components]
	
Vehicle is Implemented in
[https://github.com/howbani/VEFR/blob/master/RoadNet/Components/VehicleUi.xaml.cs] 

Data sets 
-----
Road network size is set to 4000m×4500m with 12 junctions and 17 road segments (two roadways each with 2 lanes). The traffic light is set to 5 seconds. The starting position of vehicles is randomly distributed on the road network. Vehicles travel with a maximum speed of 90kmph and minimum speed of 30kmph. 
	The intra-path data set contains 1M records, can be found via the link: [http://staff.ustc.edu.cn/~anmande/dataset/1m_intra_4lanes_400v.rar].
	The inter-path data set contains 60K records, can be found via the link: in[http://staff.ustc.edu.cn/~anmande/dataset/60k_inter.rar].
	
Modification 
-----
To implement your own protocol on our platform, follow the steps:

	Add your protocol  name in [https://github.com/howbani/VEFR/blob/master/Routing/Protcols.cs] 
	
	Your road segment selection strategy should be added to [https://github.com/howbani/VEFR/blob/master/Routing/InterRouting.cs]
	
	Your vehicle selection should be added to [https://github.com/howbani/VEFR/blob/master/Routing/IntraRouting.cs]
	

Bugs
-----
If any bugs are encountered during the execution of the toolkit, please restart toolkit, or you can download the new version of this toolkit. If there is any error occurs, the toolkit will shut down automatically.

Installation Problems
-----
1-      I can't run the Tool kit: 

        -          Install the DOT NET 4.5: Click here to download.
2-      I can't see the topologies:

     -          Toolkit can't read the topology, please install the OLEDB engine. Click here to download.
3-      If you encounter any other problems, please check your operating system capability. This toolkit is tested on windows 8 platform.

