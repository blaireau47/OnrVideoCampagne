
ALTER TABLE dbo.Soirees 
  ADD CONSTRAINT uq_Soirees UNIQUE(central, soiree);