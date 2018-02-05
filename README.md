# CryptoBot
CryptoBot that can detect arbitrages and use them to make profit.

> "The main goal of this project is to monitor the arbitrages between different markets and different cryptocurrencies.
The next step is to interact on the arbitrages and execute trades."

This project consist of several different services that have their own task.
Each service runs inside a docker container.
The services are connected over HTTP. This hard connections may be replaced with a queue in the future.

Below a diagram that shows the different services and how they are connected to each other:

<img src="Architecture_CryptoBot.jpg" alt="Architecture CryptoBot" width="50%">

### Currently supported markets
- GDAX (rate limit: 3 requests/second)
- Coinbase (rate limit: 10.000/hour -> 2.77 requests/second)
- Bittrex (rate limit: -)
- Kraken (rate limit: 1 request every 3 seconds)

### Run & Deploy
#### prerequisites
- Visual Studio 2017 (For development)
- .NET Core 2.0 (For development)
- Docker (with docker-compose)

#### For local development:
1. Clone this repository
2. Run the PricePoller project without debugging in Visual Studio. The example configuration is available in appsettings.json
3. Run the ArbitrageDetector project. Prices from the pricepoller should be picked up by the ArbitrageDetector.

#### For production:
1. Clone this repository in your production environment
2. Configure the markets and currencies of your choice (note the supported markets). Example configuration is available in docker-compose.yml.
3. Run `docker-compose up -d --build` in the root directory to build and start the docker containers.
4. Everything should be working.

#### Literature
- https://bitcoin.stackexchange.com/questions/49819/cryptocurrency-arbitrage-what-do-i-need-to-know
- https://steemit.com/arbitrage/@kesor/the-math-behind-cross-exchange-arbitrage-trading
- https://steemit.com/cryptocurrency/@scrawl/a-brief-look-at-crypto-arbitrage-trading

### TODO
- Implement ArbitrageDetector
- Add frontend to show calculated arbitrages
- Implement Fees from markets into arbitrage calculation.
