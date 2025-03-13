# E-İmza API Demo

Bu demo klasörü, E-İmza API'sini test etmek için örnek dosyalar içerir.

## Dosyalar

- `api-test.php`: PHP ile API'yi test etmek için örnek script
- `test-files/`: Test için örnek belgeler
  - `test.pdf`: Örnek PDF belgesi
  - `test.docx`: Örnek Word belgesi

## Kullanım

1. E-İmza Windows uygulamasını çalıştırın (REST API otomatik başlayacaktır)
2. PHP script'i çalıştırın:

```bash
php api-test.php
```

Script şunları yapacaktır:
- Örnek PDF ve Word belgelerini yükler
- API'ye imzalama isteği gönderir
- İmzalama durumunu kontrol eder
- İmzalı dosyaları indirir

## API Endpoints

### 1. İmzalama İsteği

```http
POST http://localhost:5000/api/signature/sign
Content-Type: application/json

{
  "documents": [
    {
      "content": "base64_encoded_content",
      "fileName": "test.pdf",
      "signature_position": {
        "x": 100,
        "y": 100
      }
    }
  ],
  "batch_id": "unique_batch_id"
}
```

### 2. Durum Kontrolü

```http
GET http://localhost:5000/api/signature/status/{batch_id}
```

## Desteklenen Dosya Formatları

- PDF (.pdf)
- Word (.docx)
- Excel (.xlsx)