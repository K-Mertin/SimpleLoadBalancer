# SimpleLoadBalancer
利用 .net core middleware 的架構，依照設定之Policy 將request 導向不同檔案服務

## 檔案說明
1. Balancer: 負載平衡器，可在 appsetting.json 中設定演算法
- RoundRobin: 輪流
- RequestSend: 已導向之數量
- WorkLoad: 找出負載量最低的服務 (Worker須要使用Queue模式)
2. BalancerPolicy: 負載平衡演算法
3. Worker: 模擬檔案服務，appsetting.json 中設定將使用的工作模式
- Directly: middleware 直接轉送request 至 worker 
- Queue: middleware 將 request 加入 worker 工作隊列中
4. Job: 模擬須執行的檔案工作

## 執行流程
1. 當接收到api request (GET /api? uri={fileUri})時，透過 BalancerMiddleware 處理負載平衡機制
2. ProxyMiddle 將 request 導向至Balancer 傳遞之檔案服務(Worker)
3. 提供 Policy api 查詢、修改負載平衡機制。
