

## ⚡ Performance Optimization Settings

Add these to your PostgreSQL configuration:

```sql
-- ============================================
-- Connection Pooling Settings
-- ============================================

-- In appsettings.json connection string:
-- Pooling=true;Minimum Pool Size=5;Maximum Pool Size=100;Connection Lifetime=300;Command Timeout=30


-- ============================================
-- PostgreSQL Configuration (postgresql.conf)
-- ============================================

-- Increase shared buffers (25% of RAM)
shared_buffers = 4GB

-- Increase effective cache size (50-75% of RAM)
effective_cache_size = 12GB

-- Increase work memory for complex queries
work_mem = 64MB

-- Increase maintenance work memory for vacuum, index creation
maintenance_work_mem = 1GB

-- Enable parallel query execution
max_parallel_workers_per_gather = 4
max_parallel_workers = 8

-- Increase checkpoint settings
checkpoint_completion_target = 0.9
wal_buffers = 16MB

-- Enable query planning optimizations
random_page_cost = 1.1  # Lower for SSDs
effective_io_concurrency = 200  # Higher for SSDs

-- Connection settings
max_connections = 200
```

---

## 📊 Monitoring Queries

Use these queries to monitor database performance in production:

```sql
-- ============================================
-- Long Running Queries
-- ============================================

SELECT 
    pid,
    now() - pg_stat_activity.query_start AS duration,
    query,
    state,
    wait_event_type,
    wait_event
FROM pg_stat_activity
WHERE (now() - pg_stat_activity.query_start) > interval '5 seconds'
  AND state = 'active'
ORDER BY duration DESC;


-- ============================================
-- Table Bloat (needs periodic vacuuming)
-- ============================================

SELECT 
    schemaname,
    tablename,
    pg_size_pretty(pg_total_relation_size(schemaname||'.'||tablename)) AS size,
    n_dead_tup AS dead_tuples,
    n_live_tup AS live_tuples,
    ROUND(n_dead_tup * 100.0 / NULLIF(n_live_tup + n_dead_tup, 0), 2) AS dead_percentage
FROM pg_stat_user_tables
WHERE schemaname IN ('catalog', 'users', 'notifications')
  AND n_dead_tup > 1000
ORDER BY dead_percentage DESC;


-- ============================================
-- Lock Monitoring
-- ============================================

SELECT 
    pg_stat_activity.pid,
    pg_class.relname,
    pg_locks.mode,
    pg_locks.granted,
    pg_stat_activity.query
FROM pg_locks
JOIN pg_class ON pg_locks.relation = pg_class.oid
JOIN pg_stat_activity ON pg_locks.pid = pg_stat_activity.pid
WHERE NOT pg_locks.granted
ORDER BY pg_stat_activity.query_start;


-- ============================================
-- Cache Hit Ratio (should be > 99%)
-- ============================================

SELECT 
    sum(heap_blks_read) as heap_read,
    sum(heap_blks_hit) as heap_hit,
    sum(heap_blks_hit) / (sum(heap_blks_hit) + sum(heap_blks_read)) * 100 AS cache_hit_ratio
FROM pg_statio_user_tables;
```

---

## 🛠️ Maintenance Scripts

```sql
-- ============================================
-- Vacuum and Analyze
-- ============================================

-- Run VACUUM ANALYZE on all tables (can run during low traffic)
VACUUM ANALYZE;

-- Or run on specific schemas
VACUUM ANALYZE catalog.orders;
VACUUM ANALYZE catalog.store_visits;
VACUUM ANALYZE users.refresh_tokens;

-- Full vacuum (requires table lock - run during maintenance window)
VACUUM FULL ANALYZE;


-- ============================================
-- Reindex
-- ============================================

-- Reindex all indexes (can help with index bloat)
REINDEX DATABASE your_database_name;

-- Or reindex specific schemas
REINDEX SCHEMA catalog;
REINDEX SCHEMA users;


-- ============================================
-- Update Statistics
-- ============================================

-- Update query planner statistics
ANALYZE;

-- Update for specific tables
ANALYZE catalog.orders;
ANALYZE catalog.payouts;
```

---
