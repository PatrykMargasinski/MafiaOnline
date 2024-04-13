CREATE VIEW VAgent AS
SELECT 
	  a.Id,
      a.BossId,
      a.LastName,
      a.FirstName,
      a.Strength,
      a.Dexterity,
      a.Intelligence,
      a.Upkeep,
      a.IsFromBossFamily,
      a.StateId,
      a.SubstateId,
	  CASE
		WHEN a.StateId = 5
		THEN ma.ArrivalTime
		WHEN a.StateId = 3
		THEN pm.CompletionTime
		ELSE NULL
	  END AS FinishTime
FROM Agent a
LEFT JOIN MovingAgent ma on ma.AgentId = a.Id
LEFT JOIN PerformingMission pm on pm.AgentId = a.Id