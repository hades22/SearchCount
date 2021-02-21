# SearchCount
1. The app starts with the UI by default. 
2. If there is a need of adding new search engine, we need to create a new SerchService which extends the abstract class SearchCountService and inplements 
   the interface ISearchCountService. 
3. The cache service is using memory cache at the moment. Ideal to move the cache to cloud based service e.g. redis.
