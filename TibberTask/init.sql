CREATE TABLE IF NOT EXISTS executions (id SERIAL, created_at timestamp with time zone, commands int, result bigint, duration float);
