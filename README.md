# Microservices Assessment POC

API üzerinden aldığı event bilgilerini RabbitMQ queue'sine iletir.

## Yapılacaklar Listesi

- RabbitMQ bağlantısını persistent ve shared bir şekilde implemente etmek
- EventBus kullanarak event bazlı mesaj gönderme
- HealthCheck entegrasyonu

## Projeyi Çalıştırma

- src klasörünün içerisinde `docker-compose up --build` çalıştırılır.
- Tarayıcı üzerinden http://localhost:3331/swagger/ adresine girilir.