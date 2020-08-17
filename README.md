
# Microservices Assessment POC

HTTP API üzerinden POST ile aldığı event bilgilerini RabbitMQ queue'sine iletir.

## Yapılacaklar Listesi

- EventBus implementasyonu
- HealthCheck implementasyonu

## Projeyi Çalıştırma

- src klasörünün içerisinde `docker-compose up --build` çalıştırılır.
- Tarayıcı üzerinden http://localhost:3331/swagger/ adresine girilir.
- RabbitMQ Management Console : http://localhost:15672 guest:guest