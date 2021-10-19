# ExchangeRESTApi

1. Crete database:
	-run in PM: add-migration start
	-run in PM: update-database

2. Run project

If want change class to get data, change AppConfig.cs from ExchangeValues = 1 to 2 to using ElasticSearch to save data no database.  But you must install and run Elasticsearch first.

You also change StartDate to store date from past. Is also in AppConfig.cs.


I use this inquiries to show data from server


![image](https://user-images.githubusercontent.com/47826375/138001863-6924e596-6fe4-4cb8-8dba-97b81d1a9d73.png)