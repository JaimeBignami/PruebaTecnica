CREATE DATABASE "PruebaTecnica";

\connect "PruebaTecnica";

CREATE TABLE "Transactions" (
  "IdCargo"   INTEGER      GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  "Pan"         VARCHAR(19), --Numero Tarjeta (hasta 19 dígitos)
  "Expiry"      VARCHAR(5),
  "Amount"      INTEGER NOT NULL,
  "Currency"    CHAR(3),     --Código Seguridad (CVV de 3 dígitos)
  "AuthorizationCode" VARCHAR(30),
  "MerchantId" VARCHAR(50), -- Id del comercio
  "Cvv" VARCHAR(4), -- Código Seguridad (CVV de 3 o 4 dígitos)
  "CreatedAt"   TIMESTAMP WITH TIME ZONE   NOT NULL
);
