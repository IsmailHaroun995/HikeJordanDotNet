-- ============================================================
-- Hike Jordan — Production test-data cleanup
-- Run against: db_acb16a_hikajordan (site4now SQL Server)
-- Preview with SELECT before running DELETE
-- ============================================================

-- 1. Preview what will be deleted
SELECT Id, Title, Organizer, Status
FROM HikeListings
WHERE LOWER(Title) LIKE '%test%'
   OR LOWER(Organizer) LIKE '%test%'
   OR LOWER(Title) = 'testtes';

-- 2. Remove test hike listings (and their reviews via cascade)
DELETE FROM HikeListings
WHERE LOWER(Title) LIKE '%test%'
   OR LOWER(Organizer) LIKE '%test%'
   OR LOWER(Title) = 'testtes';

-- 3. Remove test organizer requests
DELETE FROM OrganizerRequests
WHERE LOWER(Name) LIKE '%test%'
   OR Email LIKE '%test%';

-- 4. Preview test user accounts (keep admin)
SELECT Id, Name, Email, Role
FROM AppUsers
WHERE Email LIKE '%test%'
  AND Role != 'Admin';

-- 5. Remove test organizer accounts (keep admin@hikejordan.test)
DELETE FROM AppUsers
WHERE Email LIKE '%test%'
  AND Role != 'Admin';

-- 6. Verify remaining data looks clean
SELECT Id, Title, Organizer, Status FROM HikeListings ORDER BY Id;
SELECT Id, Name, Email, Role FROM AppUsers ORDER BY Role;
