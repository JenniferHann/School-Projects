package com.example.notes_app.model;

import com.example.notes_app.R;

/**
 * Enumeration of note categories, represented as colors.
 * @author Ian Clement (ian.clement@johnabbott.qc.ca)
 */
public enum Category {

    RED, ORANGE, YELLOW, GREEN, LIGHT_BLUE, DARK_BLUE, PURPLE, BROWN;

    public int getColorId() {
        switch (this) {
            case RED:
                return R.color.base08;
            case ORANGE:
                return R.color.base09;
            case YELLOW:
                return R.color.base0A;
            case GREEN:
                return R.color.base0B;
            case LIGHT_BLUE:
                return R.color.base0C;
            case DARK_BLUE:
                return R.color.base0D;
            case PURPLE:
                return R.color.base0E;
            case BROWN:
                return R.color.base0F;
            default:
                return R.color.base00;
        }
    }

}
