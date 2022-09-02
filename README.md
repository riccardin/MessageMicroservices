# MessageMicroservices- .NetCore + RabbitMQ

This project is composed by three projects: One for the Invoice Microservice.Another for a Payment Microservice. Then a test client will tak the place of 
the main app.  The test client will act as a front-end service with information for the Inovoice Microservice to create an invoice. Then the Invoice Microservice
will publish the message about the newly created invoice. The payment Microservice and the test client will both receive the message that the inovice was created.
For the test client, it is a confirmation the invoice was created. In a real-world scenario, it could display the invoice number to the user. The Payment Microservice servers as quick example of a downstream microservice that reacts to the creation of the invoice.

The code uses RabittMQ. It can run on a server, on your computed or a Docker container.
