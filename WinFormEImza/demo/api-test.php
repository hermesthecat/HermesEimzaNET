<?php

/**
 * E-İmza API Demo
 * Bu script örnek bir PDF dosyasını yükleyip imzalama işlemi yapar
 */

// API endpoint
$apiUrl = 'http://localhost:5000/api/signature';

// Örnek PDF dosyası
$pdfPath = 'test.pdf';

// PDF dosyasını base64'e çevir
$pdfContent = base64_encode(file_get_contents($pdfPath));

// İmzalama isteği için JSON verisi
$requestData = [
    'documents' => [
        [
            'content' => $pdfContent,
            'fileName' => 'test.pdf',
            'signature_position' => [
                'x' => 100,
                'y' => 100
            ]
        ]
    ],
    'batch_id' => uniqid('batch_'),
];

// cURL ile istek gönder
$ch = curl_init($apiUrl . '/sign');
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_POST, true);
curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($requestData));
curl_setopt($ch, CURLOPT_HTTPHEADER, [
    'Content-Type: application/json'
]);

// İsteği gönder ve yanıtı al
$response = curl_exec($ch);
$httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
curl_close($ch);

if ($httpCode === 200) {
    $result = json_decode($response, true);
    $batchId = $result['batch_id'];
    echo "İmzalama isteği gönderildi. Batch ID: " . $batchId . "\n";
    
    // İmzalama durumunu kontrol et
    do {
        sleep(2); // 2 saniye bekle
        
        $ch = curl_init($apiUrl . '/status/' . $batchId);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        
        $statusResponse = curl_exec($ch);
        $statusCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
        curl_close($ch);
        
        if ($statusCode === 200) {
            $status = json_decode($statusResponse, true);
            echo "Durum: " . $status['status'] . "\n";
            
            if ($status['status'] === 'Completed') {
                foreach ($status['results'] as $result) {
                    // İmzalı dosyayı kaydet
                    $signedContent = base64_decode($result['signedContent']);
                    file_put_contents('signed_' . $result['documentId'] . '.pdf', $signedContent);
                    echo "İmzalı dosya kaydedildi: signed_" . $result['documentId'] . ".pdf\n";
                }
                break;
            } elseif ($status['status'] === 'Failed') {
                echo "İmzalama başarısız: " . json_encode($status['results']) . "\n";
                break;
            }
        } else {
            echo "Durum kontrolü başarısız. HTTP Kodu: " . $statusCode . "\n";
            break;
        }
    } while (true);
    
} else {
    echo "İstek başarısız. HTTP Kodu: " . $httpCode . "\n";
    echo "Yanıt: " . $response . "\n";
}

// Word belgesi için örnek
$wordPath = 'test.docx';
if (file_exists($wordPath)) {
    $wordContent = base64_encode(file_get_contents($wordPath));
    
    $requestData = [
        'documents' => [
            [
                'content' => $wordContent,
                'fileName' => 'test.docx'
            ]
        ],
        'batch_id' => uniqid('batch_'),
    ];
    
    $ch = curl_init($apiUrl . '/sign');
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_POST, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($requestData));
    curl_setopt($ch, CURLOPT_HTTPHEADER, [
        'Content-Type: application/json'
    ]);
    
    $response = curl_exec($ch);
    $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);
    
    if ($httpCode === 200) {
        echo "\nWord belgesi imzalama isteği gönderildi.\n";
    }
}