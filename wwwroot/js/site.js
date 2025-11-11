// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Employee Management System - Enhanced JavaScript
$(document).ready(function() {

    // Sidebar Toggle Functionality
    const sidebar = $('#sidebar');
    const mainContent = $('.main-content');
    const sidebarToggle = $('#sidebarToggle');

    // Load sidebar state from localStorage
    const sidebarCollapsed = localStorage.getItem('sidebarCollapsed') === 'true';
    if (sidebarCollapsed) {
        sidebar.addClass('collapsed');
        mainContent.addClass('expanded');
    }

    sidebarToggle.on('click', function() {
        sidebar.toggleClass('collapsed');
        mainContent.toggleClass('expanded');

        // Save state to localStorage
        const isCollapsed = sidebar.hasClass('collapsed');
        localStorage.setItem('sidebarCollapsed', isCollapsed);

        // Animate toggle icon
        const icon = $(this).find('i');
        icon.removeClass('fa-bars fa-times');
        if (isCollapsed) {
            icon.addClass('fa-bars');
        } else {
            icon.addClass('fa-times');
        }
    });

    // Responsive sidebar for mobile
    $(window).resize(function() {
        if ($(window).width() < 768) {
            if (!sidebar.hasClass('collapsed')) {
                sidebar.addClass('collapsed');
                mainContent.addClass('expanded');
            }
        }
    });

    // Smooth scrolling for anchor links
    $('a[href^="#"]').on('click', function(event) {
        const target = $(this.getAttribute('href'));
        if (target.length) {
            event.preventDefault();
            $('html, body').stop().animate({
                scrollTop: target.offset().top - 100
            }, 500);
        }
    });

    // Enhanced tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    const tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl, {
            delay: { show: 300, hide: 100 }
        });
    });

    // Auto-hide alerts after 5 seconds
    setTimeout(function() {
        $('.alert').fadeOut('slow');
    }, 5000);

    // Form enhancement - Add loading states
    $('form').on('submit', function() {
        const submitBtn = $(this).find('button[type="submit"]');
        if (!submitBtn.hasClass('no-loading')) {
            submitBtn.prop('disabled', true);
            const originalText = submitBtn.html();
            submitBtn.html('<i class="fas fa-spinner fa-spin"></i> Processing...');
            submitBtn.data('original-text', originalText);
        }
    });

    // Reset button states on page load (in case of back button)
    $('button[type="submit"]').each(function() {
        const originalText = $(this).data('original-text');
        if (originalText) {
            $(this).html(originalText).prop('disabled', false);
        }
    });

    // Enhanced number input formatting
    $('input[type="number"]').on('input', function() {
        const value = $(this).val();
        if (value && !isNaN(value)) {
            // Add thousand separators for large numbers
            if (value > 999) {
                $(this).val(Number(value).toLocaleString());
            }
        }
    });

    // Currency input formatting
    $('input[data-type="currency"]').on('input', function() {
        let value = $(this).val().replace(/[^0-9.]/g, '');
        if (value) {
            value = parseFloat(value).toFixed(2);
            $(this).val('$' + Number(value).toLocaleString());
        }
    });

    // Phone number formatting
    $('input[type="tel"]').on('input', function() {
        let value = $(this).val().replace(/\D/g, '');
        if (value.length >= 10) {
            value = value.replace(/(\d{3})(\d{3})(\d{4})/, '($1) $2-$3');
            $(this).val(value);
        }
    });

    // Employee number auto-generation
    $('#EmployeeNumber').on('focus', function() {
        if (!$(this).val()) {
            // Generate a random employee number
            const randomNum = Math.floor(100 + Math.random() * 900);
            $(this).val('EMP' + randomNum);
        }
    });

    // Real-time email validation
    $('input[type="email"]').on('blur', function() {
        const email = $(this).val();
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (email && !emailRegex.test(email)) {
            $(this).addClass('is-invalid');
            if (!$(this).next('.invalid-feedback').length) {
                $(this).after('<div class="invalid-feedback">Please enter a valid email address.</div>');
            }
        } else {
            $(this).removeClass('is-invalid');
            $(this).next('.invalid-feedback').remove();
        }
    });

    // Password strength indicator (if password fields exist)
    $('input[type="password"]').on('input', function() {
        const password = $(this).val();
        const strengthIndicator = $(this).siblings('.password-strength');

        if (password.length > 0) {
            let strength = 0;
            if (password.length >= 8) strength++;
            if (/[A-Z]/.test(password)) strength++;
            if (/[a-z]/.test(password)) strength++;
            if (/[0-9]/.test(password)) strength++;
            if (/[^A-Za-z0-9]/.test(password)) strength++;

            strengthIndicator.show();
            strengthIndicator.removeClass('weak medium strong very-strong');

            if (strength <= 2) {
                strengthIndicator.addClass('weak').text('Weak');
            } else if (strength <= 3) {
                strengthIndicator.addClass('medium').text('Medium');
            } else if (strength <= 4) {
                strengthIndicator.addClass('strong').text('Strong');
            } else {
                strengthIndicator.addClass('very-strong').text('Very Strong');
            }
        } else {
            strengthIndicator.hide();
        }
    });

    // Table row selection
    $('.table tbody tr').on('click', function(e) {
        if (!$(e.target).is('a, button, input, select')) {
            $(this).toggleClass('selected');
            const checkbox = $(this).find('input[type="checkbox"]');
            if (checkbox.length) {
                checkbox.prop('checked', !checkbox.prop('checked'));
            }
        }
    });

    // Bulk actions for tables
    $('.bulk-action-btn').on('click', function() {
        const selectedRows = $('.table tbody tr.selected');
        if (selectedRows.length === 0) {
            alert('Please select at least one item.');
            return false;
        }

        const action = $(this).data('action');
        const ids = selectedRows.map(function() {
            return $(this).data('id');
        }).get();

        // Handle bulk actions here
        console.log('Bulk action:', action, 'for IDs:', ids);
    });

    // Search functionality with debouncing
    let searchTimeout;
    $('.search-input').on('input', function() {
        clearTimeout(searchTimeout);
        const query = $(this).val();
        const searchContainer = $(this).closest('.search-container');

        searchTimeout = setTimeout(function() {
            if (query.length >= 2) {
                searchContainer.addClass('searching');
                // Perform search here
                setTimeout(function() {
                    searchContainer.removeClass('searching');
                }, 500);
            }
        }, 300);
    });

    // Modal enhancements
    $('.modal').on('shown.bs.modal', function() {
        $(this).find('input:first').focus();
    });

    // Print functionality
    $('.print-btn').on('click', function() {
        window.print();
    });

    // Export functionality
    $('.export-btn').on('click', function() {
        const format = $(this).data('format') || 'csv';
        const table = $(this).closest('.card').find('table');

        if (format === 'csv') {
            exportToCSV(table);
        } else if (format === 'excel') {
            exportToExcel(table);
        }
    });

    function exportToCSV(table) {
        let csv = [];
        const rows = table.find('tr');

        rows.each(function() {
            const row = [];
            $(this).find('th, td').each(function() {
                row.push('"' + $(this).text().trim() + '"');
            });
            csv.push(row.join(','));
        });

        const csvContent = csv.join('\n');
        const blob = new Blob([csvContent], { type: 'text/csv' });
        const url = window.URL.createObjectURL(blob);

        const a = document.createElement('a');
        a.href = url;
        a.download = 'employees.csv';
        a.click();
        window.URL.revokeObjectURL(url);
    }

    function exportToExcel(table) {
        // For Excel export, we'd typically use a library like SheetJS
        // For now, fall back to CSV
        exportToCSV(table);
    }

    // Dark mode toggle (future enhancement)
    $('.dark-mode-toggle').on('click', function() {
        $('body').toggleClass('dark-mode');
        const isDark = $('body').hasClass('dark-mode');
        localStorage.setItem('darkMode', isDark);
    });

    // Load dark mode preference
    if (localStorage.getItem('darkMode') === 'true') {
        $('body').addClass('dark-mode');
    }

    // Loading spinner for AJAX requests
    $(document).ajaxStart(function() {
        $('#loading-spinner').show();
    });

    $(document).ajaxStop(function() {
        $('#loading-spinner').hide();
    });

    // Keyboard shortcuts
    $(document).on('keydown', function(e) {
        // Ctrl/Cmd + Enter to submit forms
        if ((e.ctrlKey || e.metaKey) && e.keyCode === 13) {
            $('form:visible button[type="submit"]:first').click();
        }

        // Escape to close modals
        if (e.keyCode === 27) {
            $('.modal.show').modal('hide');
        }
    });

    // Animate elements on scroll
    const animateOnScroll = function() {
        $('.animate-on-scroll').each(function() {
            const elementTop = $(this).offset().top;
            const elementBottom = elementTop + $(this).outerHeight();
            const viewportTop = $(window).scrollTop();
            const viewportBottom = viewportTop + $(window).height();

            if (elementBottom > viewportTop && elementTop < viewportBottom) {
                $(this).addClass('animated');
            }
        });
    };

    $(window).on('scroll', animateOnScroll);
    animateOnScroll(); // Initial check

    // Notification system
    function showNotification(message, type = 'info') {
        const notification = $(`
            <div class="notification notification-${type}">
                <i class="fas fa-${type === 'success' ? 'check-circle' : type === 'error' ? 'exclamation-circle' : 'info-circle'}"></i>
                ${message}
                <button class="notification-close">&times;</button>
            </div>
        `);

        $('.notification-container').append(notification);

        setTimeout(function() {
            notification.addClass('show');
        }, 100);

        notification.find('.notification-close').on('click', function() {
            notification.removeClass('show');
            setTimeout(function() {
                notification.remove();
            }, 300);
        });

        setTimeout(function() {
            notification.removeClass('show');
            setTimeout(function() {
                notification.remove();
            }, 300);
        }, 5000);
    }

    // Make showNotification globally available
    window.showNotification = showNotification;

    // Performance optimization: Lazy load images
    const lazyImages = $('img[data-src]');
    const imageObserver = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const img = entry.target;
                img.src = img.dataset.src;
                img.classList.remove('lazy');
                observer.unobserve(img);
            }
        });
    });

    lazyImages.each(function() {
        imageObserver.observe(this);
    });

    // Accessibility improvements
    $('.skip-link').on('click', function(e) {
        e.preventDefault();
        const target = $(this).attr('href');
        $(target).attr('tabindex', -1).focus();
    });

    // High contrast mode toggle
    $('.high-contrast-toggle').on('click', function() {
        $('body').toggleClass('high-contrast');
        localStorage.setItem('highContrast', $('body').hasClass('high-contrast'));
    });

    if (localStorage.getItem('highContrast') === 'true') {
        $('body').addClass('high-contrast');
    }

    console.log('Employee Management System JavaScript loaded successfully!');
});
