S3FileRepository
================

A C# implementation of a AWS S3 file repository. This repository lets .NET developers access a given S3 bucket to fetch the folders 
and files, download a file, upload a file, check the existance of a file, delete a file etc.

To begin using the library you will need to add the required configuration items to your web.config or app.config file. The configuration item will require your secredIt, accessId and region endpoint that AWS will provide you.  The below regions names can be used in the configuration file:


Region Name				                    Region	Endpoint	
US East (N. Virginia) region		      us-east-1	
US West (N. California) region		    us-west-1	
US West (Oregon) region		          	us-west-2	
EU (Ireland) region			              eu-west-1	
Asia Pacific (Singapore) region		    ap-southeast-1	
Asia Pacific (Sydney) region		      ap-southeast-2	
Asia Pacific (Tokyo) region		        ap-northeast-1	
South America (Sao Paulo) region	    sa-east-1	

For more details please see my blog which is http://aspguy.wordpress.com

