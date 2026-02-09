CREATE TABLE IF NOT EXISTS policy (
    id SERIAL PRIMARY KEY,
    policy_name VARCHAR(255) NOT NULL,
    description TEXT,
    premium_amount INTEGER,
    is_active BOOLEAN,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);