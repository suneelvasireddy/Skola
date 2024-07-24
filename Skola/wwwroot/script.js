document.addEventListener('DOMContentLoaded', () => {
    const schoolList = document.getElementById('school-list');
    const studentList = document.getElementById('student-list');
    const errorMessage = document.getElementById('error-message');
    const schoolNameInput = document.getElementById('school-name');
    const getStudentsBtn = document.getElementById('get-students-btn');
    const paginationContainer = document.getElementById('pagination');
    const resultsPerPageSelect = document.getElementById('results-per-page');

    let currentPage = 1;
    let pageSize = parseInt(resultsPerPageSelect.value, 10);

    getSchools();

    // Event listener for results per page dropdown
    resultsPerPageSelect.addEventListener('change', () => {
        pageSize = parseInt(resultsPerPageSelect.value, 10);
        currentPage = 1; // Reset to the first page when changing page size
        if (schoolNameInput.value.trim()) {
            getStudents(schoolNameInput.value.trim());
        }
    });

    // Function to fetch schools and populate the left pane
    async function getSchools() {
        try {
            const response = await fetch('/api/schools');
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const schools = await response.json();
            populateSchools(schools);
        } catch (error) {
            console.error('Error fetching schools:', error);
            displayError('Failed to fetch schools. Please try again.');
        }
    }

    // Function to populate the left pane with school names
    function populateSchools(schools) {
        schoolList.innerHTML = '';
        schools.forEach(school => {
            const li = document.createElement('li');
            li.textContent = school; // Displaying only the name property of the school object
            li.addEventListener('click', () => {
                schoolNameInput.value = school; // Populate input with school name
                getStudents(school); // Fetch students for the selected school
                studentList.scrollTop = 0; // Scroll to top of student list
            });
            schoolList.appendChild(li);
        });
    }

    // Event listener for Get Students button
    getStudentsBtn.addEventListener('click', () => {
        const schoolName = schoolNameInput.value.trim();
        if (schoolName) {
            currentPage = 1; // Reset current page when fetching new students
            getStudents(schoolName);
        } else {
            displayError('Please enter a school name.');
        }
    });

    // Function to fetch students for a selected school
    async function getStudents(schoolName) {
        try {
            // Clear previous results and error message
            studentList.innerHTML = '';
            errorMessage.textContent = '';
            errorMessage.classList.remove('active');

            // Fetch students with pagination
            const response = await fetch(`/api/students/students?schoolName=${encodeURIComponent(schoolName)}&pageNumber=${currentPage}&pageSize=${pageSize}`);

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const students = await response.json();

            if (students.data.Items.length === 0) {
                displayError('No students found for the provided school name. Please check the school name and try again.');
            } else {
                populateStudents(students.data.Items); // Use students.items to populate
                setupPagination(students.data.TotalCount); // Pass totalCount for pagination setup
            }

        } catch (error) {
            console.error('Error fetching students:', error);
            displayError('Failed to fetch students. Please try again.');
        }
    }

    // Function to populate the right pane with students
    function populateStudents(students) {
        studentList.innerHTML = '';
        students.forEach(student => {
            const li = document.createElement('li');
            li.textContent = `${student.Name} - Absent Days: ${student.TotalAbsentDays}`;
            studentList.appendChild(li);
        });
    }

    // Function to display error message
    function displayError(message) {
        errorMessage.textContent = message;
        errorMessage.classList.add('active');
    }

    // Function to set up pagination controls
    function setupPagination(totalStudents) {
        paginationContainer.innerHTML = '';
        const totalPages = Math.ceil(totalStudents / pageSize);

        // First button
        const firstButton = createPaginationButton('First', () => {
            currentPage = 1;
            getStudents(schoolNameInput.value.trim());
        });
        paginationContainer.appendChild(firstButton);

        // Previous button
        const prevButton = createPaginationButton('Prev', () => {
            if (currentPage > 1) {
                currentPage--;
                getStudents(schoolNameInput.value.trim());
            }
        });
        paginationContainer.appendChild(prevButton);

        // Calculate start and end of pagination range
        let start = Math.max(1, currentPage - 3);
        let end = Math.min(totalPages, currentPage + 3);

        // Ensure we have exactly 7 pages if possible
        if (end - start < 6) {
            if (start === 1) {
                end = Math.min(totalPages, start + 6);
            } else {
                start = Math.max(1, end - 6);
            }
        }

        // Page buttons
        for (let i = start; i <= end; i++) {
            const pageButton = createPaginationButton(i, () => {
                currentPage = i;
                getStudents(schoolNameInput.value.trim());
            });
            paginationContainer.appendChild(pageButton);
        }

        // Next button
        const nextButton = createPaginationButton('Next', () => {
            if (currentPage < totalPages) {
                currentPage++;
                getStudents(schoolNameInput.value.trim());
            }
        });
        paginationContainer.appendChild(nextButton);

        // Last button
        const lastButton = createPaginationButton('Last', () => {
            currentPage = totalPages;
            getStudents(schoolNameInput.value.trim());
        });
        paginationContainer.appendChild(lastButton);

        // Center pagination buttons
        paginationContainer.style.textAlign = 'center';
    }

    // Helper function to create pagination buttons
    function createPaginationButton(label, onClick) {
        const button = document.createElement('button');
        button.textContent = label;
        button.addEventListener('click', onClick);
        return button;
    }
});
