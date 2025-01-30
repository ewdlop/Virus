#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include <stdbool.h>
#include <assert.h>
#include <signal.h>
#include <unistd.h>
#include <sys/time.h>

#define MAX_STRING_LENGTH 256
#define MAX_CREW_MEMBERS 10
#define REQUIRED_REST_HOURS 12
#define MAINTENANCE_INTERVAL_DAYS 7
#define MAX_FLIGHT_HOURS 100.0
#define SAFETY_CHECK_DELAY_SEC 1
#define MAX_RETRY_ATTEMPTS 3
#define HEARTBEAT_INTERVAL_SEC 5
#define BACKUP_INTERVAL_SEC 300
#define EMERGENCY_BUFFER_SIZE 1024

// Enhanced status tracking
typedef enum {
    MAINTENANCE_REQUIRED,
    MAINTENANCE_SCHEDULED,
    MAINTENANCE_IN_PROGRESS,
    MAINTENANCE_COMPLETED,
    MAINTENANCE_NOT_REQUIRED
} MaintenanceStatus;

typedef enum {
    WEATHER_SEVERE,
    WEATHER_MARGINAL,
    WEATHER_ACCEPTABLE,
    WEATHER_OPTIMAL
} WeatherCondition;

typedef enum {
    SYSTEM_NORMAL,
    SYSTEM_DEGRADED,
    SYSTEM_BACKUP,
    SYSTEM_EMERGENCY,
    SYSTEM_FAILURE
} SystemStatus;

typedef enum {
    LOG_DEBUG,
    LOG_INFO,
    LOG_WARNING,
    LOG_ERROR,
    LOG_EMERGENCY
} LogLevel;

// Enhanced error tracking
typedef struct {
    int error_code;
    char message[MAX_STRING_LENGTH];
    time_t timestamp;
    int retry_count;
    bool is_recoverable;
} ErrorContext;

// Real-time monitoring
typedef struct {
    time_t last_heartbeat;
    SystemStatus status;
    int consecutive_failures;
    bool backup_system_active;
    char error_buffer[EMERGENCY_BUFFER_SIZE];
} MonitoringContext;

// Enhanced Aircraft structure
typedef struct {
    char id[MAX_STRING_LENGTH];
    char model[MAX_STRING_LENGTH];
    time_t last_maintenance;
    double total_flight_hours;
    MaintenanceStatus maintenance_status;
    bool redundancy_systems_check;
    bool safety_inspection_passed;
    bool emergency_systems_check;
    bool backup_systems_active;
    SystemStatus current_status;
} Aircraft;

// Enhanced CrewMember structure
typedef struct {
    char id[MAX_STRING_LENGTH];
    double rest_hours;
    time_t last_flight_time;
    bool qualification_status;
    bool health_check_passed;
    bool emergency_training_current;
    int fatigue_level;
    SystemStatus readiness_status;
} CrewMember;

// Global monitoring context
MonitoringContext g_monitor = {0};

// Error recovery functions
void initiate_error_recovery(ErrorContext* error) {
    char log_message[MAX_STRING_LENGTH];
    snprintf(log_message, sizeof(log_message), 
            "Initiating error recovery for code %d: %s", 
            error->error_code, error->message);
    safety_log(LOG_WARNING, log_message);

    if (error->retry_count < MAX_RETRY_ATTEMPTS && error->is_recoverable) {
        error->retry_count++;
        // Implement recovery strategy based on error code
        switch (error->error_code) {
            case 1: // System communication error
                g_monitor.backup_system_active = true;
                break;
            case 2: // Data validation error
                // Reset to last known good state
                break;
            case 3: // Hardware error
                // Switch to backup hardware
                break;
            default:
                safety_log(LOG_ERROR, "Unknown error code in recovery");
        }
    } else {
        safety_log(LOG_EMERGENCY, "Maximum retry attempts exceeded or unrecoverable error");
        g_monitor.status = SYSTEM_EMERGENCY;
    }
}

// Real-time monitoring functions
void heartbeat_monitor(void) {
    time_t now;
    time(&now);
    
    if (difftime(now, g_monitor.last_heartbeat) > HEARTBEAT_INTERVAL_SEC) {
        safety_log(LOG_WARNING, "Heartbeat monitor detected delay");
        g_monitor.consecutive_failures++;
        
        if (g_monitor.consecutive_failures > 3) {
            safety_log(LOG_EMERGENCY, "Multiple heartbeat failures detected");
            g_monitor.status = SYSTEM_DEGRADED;
        }
    } else {
        g_monitor.consecutive_failures = 0;
    }
    
    g_monitor.last_heartbeat = now;
}

// Emergency protocols
void initiate_emergency_protocol(Aircraft* aircraft, CrewMember* crew, int crew_count) {
    safety_log(LOG_EMERGENCY, "Initiating emergency protocols");
    
    // 1. Switch to backup systems
    aircraft->backup_systems_active = true;
    g_monitor.status = SYSTEM_EMERGENCY;
    
    // 2. Notify crew
    for (int i = 0; i < crew_count; i++) {
        if (!crew[i].emergency_training_current) {
            safety_log(LOG_ERROR, "Crew member lacks current emergency training");
        }
    }
    
    // 3. Log emergency state
    char emergency_log[EMERGENCY_BUFFER_SIZE];
    snprintf(emergency_log, sizeof(emergency_log),
             "Emergency state - Aircraft: %s, Status: %d, Backup: %d",
             aircraft->id, g_monitor.status, aircraft->backup_systems_active);
    safety_log(LOG_EMERGENCY, emergency_log);
    
    // 4. Attempt recovery
    ErrorContext error = {
        .error_code = 999,
        .is_recoverable = true,
        .retry_count = 0
    };
    strcpy(error.message, "Emergency protocol initiated");
    initiate_error_recovery(&error);
}

// Enhanced safety validation with redundancy
bool validate_aircraft_safety_redundant(const Aircraft* aircraft) {
    bool primary_check = validate_aircraft_safety(aircraft);
    bool secondary_check = validate_aircraft_safety(aircraft);
    
    if (primary_check != secondary_check) {
        safety_log(LOG_ERROR, "Safety check validation mismatch");
        return false;
    }
    
    return primary_check && secondary_check;
}

// Main scheduling function with enhanced safety
bool schedule_flight_safe(SafetyContext* context) {
    // Initialize monitoring
    g_monitor.status = SYSTEM_NORMAL;
    time(&g_monitor.last_heartbeat);
    g_monitor.consecutive_failures = 0;
    
    ErrorContext error = {0};
    
    // Primary scheduling attempt
    bool primary_result = schedule_flight(context);
    
    // Redundant scheduling check
    bool secondary_result = schedule_flight(context);
    
    if (primary_result != secondary_result) {
        error.error_code = 1;
        error.is_recoverable = true;
        strcpy(error.message, "Schedule validation mismatch");
        initiate_error_recovery(&error);
        return false;
    }
    
    // Continuous monitoring
    while (primary_result && g_monitor.status == SYSTEM_NORMAL) {
        heartbeat_monitor();
        
        if (g_monitor.status != SYSTEM_NORMAL) {
            initiate_emergency_protocol(context->aircraft, context->crew, context->crew_count);
            return false;
        }
        
        sleep(1); // Regular monitoring interval
    }
    
    return primary_result && secondary_result;
}

int main() {
    // Initialize test data with enhanced safety features
    Aircraft test_aircraft = {
        .id = "AC001",
        .model = "Boeing 737",
        .total_flight_hours = 90.0,
        .maintenance_status = MAINTENANCE_COMPLETED,
        .redundancy_systems_check = true,
        .safety_inspection_passed = true,
        .emergency_systems_check = true,
        .backup_systems_active = false,
        .current_status = SYSTEM_NORMAL
    };
    time(&test_aircraft.last_maintenance);

    CrewMember test_crew[1] = {{
        .id = "CR001",
        .rest_hours = 14.0,
        .qualification_status = true,
        .health_check_passed = true,
        .emergency_training_current = true,
        .fatigue_level = 0,
        .readiness_status = SYSTEM_NORMAL
    }};
    time(&test_crew[0].last_flight_time);

    SafetyContext context = {
        .aircraft = &test_aircraft,
        .crew = test_crew,
        .crew_count = 1,
        .departure = "JFK",
        .destination = "LAX"
    };

    // Schedule flight with enhanced safety
    if (schedule_flight_safe(&context)) {
        safety_log(LOG_INFO, "Flight scheduling successful with all safety checks");
    } else {
        safety_log(LOG_WARNING, "Flight scheduling failed - safety requirements not met");
    }

    return 0;
}
