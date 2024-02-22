package com.example.notes_app;

import android.app.DatePickerDialog;
import android.app.TimePickerDialog;
import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.Switch;
import android.widget.TextView;
import android.widget.TimePicker;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;

import com.example.notes_app.model.Category;
import com.example.notes_app.model.Note;

import java.util.*;

public class NoteEditFragment extends Fragment {

    //all text views
    private EditText titleEditText;
    private EditText bodyEditText;
    private TextView reminderTextView;

    //all circle views for colour category
    private com.example.notes_app.CircleView redCircleView;
    private com.example.notes_app.CircleView orangeCircleView;
    private com.example.notes_app.CircleView yellowCircleView;
    private com.example.notes_app.CircleView greenCircleView;
    private com.example.notes_app.CircleView lightBlueCircleView;
    private com.example.notes_app.CircleView darkBlueCircleView;
    private com.example.notes_app.CircleView purpleCircleView;
    private com.example.notes_app.CircleView brownCircleView;

    //button for undo
    private View undoButton;

    //switch for note properties
    private Switch notePropertiesSwitch;

    //layout of note app
    private View mainLayout;
    private View allColoursLayout;
    private View bodyLayout;
    private View titleLayout;

    //string for hint in text views
    private String titleHint;
    private String bodyHint;
    private String reminderHint;

    //check state of switch to show note properties
    private boolean switchState;

    //save all modifications
    private List undos;

    //save title of note
    private String title;

    //save body of note
    private String body;

    //save colour category of note
    private Category category;

    //save date of reminder of note
    private Date reminder;

    //check if note has a reminder
    private boolean hasReminder;

    //check if undo button was pressed
    private boolean hasUndo;

    @Override
    public View onCreateView( LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View root = inflater.inflate(R.layout.fragment_first, container, false);

        //get all views from fragment
        titleEditText = root.findViewById(R.id.title_EditText);
        bodyEditText = root.findViewById(R.id.body_EditText);
        reminderTextView = root.findViewById(R.id.reminder_TextView);
        redCircleView = root.findViewById(R.id.red_Circleview);
        orangeCircleView = root.findViewById(R.id.orange_Circleview);
        yellowCircleView = root.findViewById(R.id.yellow_Circleview);
        greenCircleView = root.findViewById(R.id.green_Circleview);
        lightBlueCircleView = root.findViewById(R.id.lightBlue_Circleview);
        darkBlueCircleView = root.findViewById(R.id.blue_Circleview);
        purpleCircleView = root.findViewById(R.id.purple_Circleview);
        brownCircleView = root.findViewById(R.id.brown_Circleview);
        undoButton = root.findViewById(R.id.undo_Button);
        notePropertiesSwitch = root.findViewById(R.id.noteProperties_Switch);

        //get all layout
        mainLayout = root.findViewById(R.id.MainLayout);
        allColoursLayout = root.findViewById(R.id.linearLayout_allColours);
        bodyLayout = root.findViewById(R.id.textInputLayout_body);
        titleLayout = root.findViewById(R.id.textInputLayout_title);

        //add watcher to title and body text
        titleEditText.addTextChangedListener(new TitleEditTextWatcher());
        bodyEditText.addTextChangedListener(new BodyEditTextWatcher());

        //red circle clicked
        redCircleView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                //change colour of background
                titleLayout.setBackgroundColor(getResources().getColor(Category.RED.getColorId()));
                bodyLayout.setBackgroundColor(getResources().getColor(Category.RED.getColorId()));

                //create new note with current title, body, reminder and colour category
                Note noteCategory = new Note();
                noteCategory.setTitle(title);
                noteCategory.setBody(body);
                category = Category.RED;
                noteCategory.setCategory(category);
                noteCategory.setHasReminder(hasReminder);
                noteCategory.setReminder(reminder);

                //add new modified note to list of note
                undos.add(noteCategory);
            }
        });

        //orange circle clicked
        orangeCircleView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                //change colour of background
                titleLayout.setBackgroundColor(getResources().getColor(Category.ORANGE.getColorId()));
                bodyLayout.setBackgroundColor(getResources().getColor(Category.ORANGE.getColorId()));

                //create new note with current title, body, reminder and colour category
                Note noteCategory = new Note();
                noteCategory.setTitle(title);
                noteCategory.setBody(body);
                category = Category.ORANGE;
                noteCategory.setCategory(category);
                noteCategory.setHasReminder(hasReminder);
                noteCategory.setReminder(reminder);

                //add new modified note to list of note
                undos.add(noteCategory);
            }
        });

        yellowCircleView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                titleLayout.setBackgroundColor(getResources().getColor(Category.YELLOW.getColorId()));
                bodyLayout.setBackgroundColor(getResources().getColor(Category.YELLOW.getColorId()));

                Note noteCategory = new Note();
                noteCategory.setTitle(title);
                noteCategory.setBody(body);
                category = Category.YELLOW;
                noteCategory.setCategory(category);
                noteCategory.setHasReminder(hasReminder);
                noteCategory.setReminder(reminder);
                undos.add(noteCategory);
            }
        });

        greenCircleView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                titleLayout.setBackgroundColor(getResources().getColor(Category.GREEN.getColorId()));
                bodyLayout.setBackgroundColor(getResources().getColor(Category.GREEN.getColorId()));

                Note noteCategory = new Note();
                noteCategory.setTitle(title);
                noteCategory.setBody(body);
                category = Category.GREEN;
                noteCategory.setCategory(category);
                noteCategory.setHasReminder(hasReminder);
                noteCategory.setReminder(reminder);
                undos.add(noteCategory);
            }
        });

        lightBlueCircleView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                titleLayout.setBackgroundColor(getResources().getColor(Category.LIGHT_BLUE.getColorId()));
                bodyLayout.setBackgroundColor(getResources().getColor(Category.LIGHT_BLUE.getColorId()));

                Note noteCategory = new Note();
                noteCategory.setTitle(title);
                noteCategory.setBody(body);
                category = Category.LIGHT_BLUE;
                noteCategory.setCategory(category);
                noteCategory.setHasReminder(hasReminder);
                noteCategory.setReminder(reminder);
                undos.add(noteCategory);
            }
        });

        darkBlueCircleView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                titleLayout.setBackgroundColor(getResources().getColor(Category.DARK_BLUE.getColorId()));
                bodyLayout.setBackgroundColor(getResources().getColor(Category.DARK_BLUE.getColorId()));

                Note noteCategory = new Note();
                noteCategory.setTitle(title);
                noteCategory.setBody(body);
                category = Category.DARK_BLUE;
                noteCategory.setCategory(category);
                noteCategory.setHasReminder(hasReminder);
                noteCategory.setReminder(reminder);
                undos.add(noteCategory);
            }
        });

        purpleCircleView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                titleLayout.setBackgroundColor(getResources().getColor(Category.PURPLE.getColorId()));
                bodyLayout.setBackgroundColor(getResources().getColor(Category.PURPLE.getColorId()));

                Note noteCategory = new Note();
                noteCategory.setTitle(title);
                noteCategory.setBody(body);
                category = Category.PURPLE;
                noteCategory.setCategory(category);
                noteCategory.setHasReminder(hasReminder);
                noteCategory.setReminder(reminder);
                undos.add(noteCategory);
            }
        });

        brownCircleView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                titleLayout.setBackgroundColor(getResources().getColor(Category.BROWN.getColorId()));
                bodyLayout.setBackgroundColor(getResources().getColor(Category.BROWN.getColorId()));

                Note noteCategory = new Note();
                noteCategory.setTitle(title);
                noteCategory.setBody(body);
                category = Category.BROWN;
                noteCategory.setCategory(category);
                noteCategory.setHasReminder(hasReminder);
                noteCategory.setReminder(reminder);
                undos.add(noteCategory);
            }
        });

        //listener for switch to show reminder and colour options
        notePropertiesSwitch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //switch is turn on, hide reminder and note colour changer
                if (switchState == true) {
                    //hide reminder and colour selection
                    reminderTextView.setVisibility(View.GONE);
                    allColoursLayout.setVisibility(View.GONE);
                    //reset switch
                    switchState = false;
                }
                else {  //switch turn off, show reminder and note colour changer
                    reminderTextView.setVisibility(View.VISIBLE);
                    allColoursLayout.setVisibility(View.VISIBLE);
                    switchState = true;
                }
            }
        });

        //undo button was clicked
        undoButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                //minimum of note in list of undo
                int min = 1;
                //undo button was clicked
                hasUndo = true;
                //save previous note before last modifications
                Note previousNote = new Note();

                //get current note
                Note currentNote = new Note();
                currentNote.setTitle(title);
                currentNote.setBody(body);
                currentNote.setCategory(category);
                currentNote.setHasReminder(hasReminder);
                currentNote.setReminder(reminder);

                //check if note is note going back to default note
                if (undos.size() > min) {

                    //check if current note is note the same as the latest modification saved
                    if ( (undos.get(undos.size()-1).equals(currentNote) )) {
                        //remove same modification
                        undos.remove(undos.size()-1);
                    }

                    //check if default note is still there as the first note in the list
                    if (undos.size() > min) {
                        //default note is still there
                        //get previous modification
                        previousNote = (Note) undos.remove(undos.size()-1);
                    }
                    else if (undos.size() == min) {     //back to default note
                        //get previous note
                        previousNote = (Note) undos.get(undos.size()-1);
                    }
                }
                else if (undos.size() == min) {     //back to default note
                    //get default note
                    previousNote = (Note) undos.get(undos.size()-1);
                }

                //set current note to previous modified note
                currentNote = previousNote;

                //get and set the note title
                if (previousNote.getTitle() != null) {
                    titleEditText.setText(previousNote.getTitle());
                    title = currentNote.getTitle();
                }
                else {
                    titleEditText.setText("");
                    title = "";
                }

                //get and set the note body
                if (previousNote.getBody() != null) {
                    bodyEditText.setText(previousNote.getBody());
                    body = currentNote.getBody();
                }
                else {
                    bodyEditText.setText("");
                    body = "";
                }

                //get and set the note colour category
                if (previousNote.getCategory() != null) {
                    titleLayout.setBackgroundColor(getResources().getColor(previousNote.getCategory().getColorId()));
                    bodyLayout.setBackgroundColor(getResources().getColor(previousNote.getCategory().getColorId()));
                    category = currentNote.getCategory();
                }
                else if (previousNote.getCategory() == null) {
                    titleLayout.setBackgroundColor(getResources().getColor(R.color.white));
                    bodyLayout.setBackgroundColor(getResources().getColor(R.color.white));
                    category = null;
                }

                //get and set the note reminder
                if (previousNote.isHasReminder() == true) {
                    reminderTextView.setText("Reminder: " + previousNote.getReminder().toString());
                    hasReminder = true;
                    reminder = currentNote.getReminder();
                }
                else {
                    reminderTextView.setText(reminderHint);
                    hasReminder = false;
                    reminder = null;
                }

                //change colour of note reminder to white to make it visible with black background
                reminderTextView.setTextColor(getResources().getColor(R.color.white));
            }
        });

        //user want to add a reminder
        reminderTextView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //choose a date for the reminder
                chooseDate();
            }
        });

        //assign hint value for title, body and reminder
        titleHint = "Title";
        bodyHint = "Body";
        reminderHint = "Add a reminder";

        //set default for switch on, showing reminder and colour option
        switchState = true;

        //initialize undo list
        undos = new ArrayList();
        //initialize date reminder
        reminder = null;
        //set default note doesn't have a reminder
        hasReminder = false;
        //set default note that no undos were made
        hasUndo = false;

        //creating and saving default note
        Note note = new Note();
        note.setTitle(title);
        note.setBody(body);
        note.setHasReminder(hasReminder);
        note.setCategory(null);
        undos.add(note);

        //creating default look of the note app
        defaultStarter();

        return root;
    }

    public void
    onViewCreated(@NonNull View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
    }

    //set default colours and text for note
    public void defaultStarter() {
        titleEditText.setHint(titleHint);
        bodyEditText.setHint(bodyHint);
        reminderTextView.setHint(reminderHint);
        reminderTextView.setHintTextColor(getResources().getColor(R.color.white));
        mainLayout.setBackgroundColor(getResources().getColor(R.color.base00));
        titleLayout.setBackgroundColor(getResources().getColor(R.color.white));
        bodyLayout.setBackgroundColor(getResources().getColor(R.color.white));
        redCircleView.setColor(getResources().getColor(Category.RED.getColorId()));
        orangeCircleView.setColor(getResources().getColor(Category.ORANGE.getColorId()));
        yellowCircleView.setColor(getResources().getColor(Category.YELLOW.getColorId()));
        greenCircleView.setColor(getResources().getColor(Category.GREEN.getColorId()));
        lightBlueCircleView.setColor(getResources().getColor(Category.LIGHT_BLUE.getColorId()));
        darkBlueCircleView.setColor(getResources().getColor(Category.DARK_BLUE.getColorId()));
        purpleCircleView.setColor(getResources().getColor(Category.PURPLE.getColorId()));
        brownCircleView.setColor(getResources().getColor(Category.BROWN.getColorId()));
        notePropertiesSwitch.setChecked(true);
    }

    private class TitleEditTextWatcher implements TextWatcher {
        @Override
        public void beforeTextChanged(CharSequence charSequence, int i, int i1, int i2) { }

        @Override
        public void onTextChanged(CharSequence charSequence, int i, int i1, int i2) {
            //save title entered by user
            try {
                title = titleEditText.getText().toString();
            } catch (Exception e) {
                title = "";
            }

        }

        @Override
        public void afterTextChanged(Editable editable) {

            //check if there was no undos to save current body text
            if (hasUndo == false) {

                //saving current state of note
                Note noteTitle = new Note();
                noteTitle.setTitle(title);
                noteTitle.setBody(body);
                noteTitle.setCategory(category);
                noteTitle.setHasReminder(hasReminder);
                noteTitle.setReminder(reminder);

                //adding changes to list of undos
                undos.add(noteTitle);
            }
        }
    }

    private class BodyEditTextWatcher implements TextWatcher {
        @Override
        public void beforeTextChanged(CharSequence charSequence, int i, int i1, int i2) {
        }

        @Override
        public void onTextChanged(CharSequence charSequence, int i, int i1, int i2) {
            //save body entered by user
            try {
                body = bodyEditText.getText().toString();
            } catch (Exception e) {
                body = "";
            }

        }

        @Override
        public void afterTextChanged(Editable editable) {

            //check if there was no undos to save current body text
            if (hasUndo == false) {

                //saving current state of note
                Note noteBody = new Note();
                noteBody.setTitle(title);
                noteBody.setBody(body);
                noteBody.setCategory(category);
                noteBody.setHasReminder(hasReminder);
                noteBody.setReminder(reminder);

                //adding changes to list of undos
                undos.add(noteBody);
            }
            else {
                //reset switch that check if the undo button was clicked
                hasUndo = false;
            }
        }
    }

    //allow user to choose a date
    private void chooseDate() {
        Date now = new Date();
        com.example.notes_app.DatePickerDialogFragment dialogFragment = com.example.notes_app.DatePickerDialogFragment.create(now, new DatePickerDialog.OnDateSetListener() {
            @Override
            public void onDateSet(DatePicker datePicker, int year, int month, int dayOfMonth) {
                Calendar calendar = Calendar.getInstance();
                calendar.set(year, month, dayOfMonth);

                //send date chosen by user to let user choose a time for note reminder
                chooseTime(year, month, dayOfMonth);
            }
        });
        dialogFragment.show(getFragmentManager(), "datePicker");
    }

    //allow user to chose a time
    private void chooseTime(final int YEAR, final int MONTH, final int DAY_OF_MONTH) {
        Date now = new Date();
        com.example.notes_app.TimePickerDialogFragment dialogFragment = com.example.notes_app.TimePickerDialogFragment.create(now, new TimePickerDialog.OnTimeSetListener() {
            @Override
            public void onTimeSet(TimePicker timePicker, int hourOfDay, int minute) {
                Calendar calendar = Calendar.getInstance();
                calendar.set(Calendar.HOUR, hourOfDay);
                calendar.set(Calendar.MINUTE, minute);

                //creating and saving current modified note to list of modified note for undoing changes
                Note noteReminder = new Note();
                noteReminder.setTitle(title);
                noteReminder.setBody(body);
                hasReminder = true;
                noteReminder.setHasReminder(hasReminder);
                noteReminder.setCategory(category);
                //creating new date to be set as reminder in this note
                Date date = new Date();
                date.setYear(YEAR);
                date.setMonth(MONTH);
                date.setDate(DAY_OF_MONTH);
                date.setHours(hourOfDay);
                date.setMinutes(minute);
                noteReminder.setReminder(date);

                //adding modified note to list of changes
                undos.add(noteReminder);

                //display date reminder to user
                reminderTextView.setText("Reminder: " + date.toString());
                reminderTextView.setTextColor(getResources().getColor(R.color.white));

                //save current date
                reminder = date;
            }
        });
        dialogFragment.show(getFragmentManager(), "timePicker");
    }
}